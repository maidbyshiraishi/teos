using Godot;
using maid_by_shiraishi.data;
using maid_by_shiraishi.path;
using maid_by_shiraishi.screen;
using maid_by_shiraishi.stage.character_manager;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.stage;

/// <summary>
/// ゲームステージの親
/// </summary>
public partial class GameStageRoot : DialogRoot, IStateful
{
    public static readonly string ProcessGroup = "ProcessGroup";
    public static readonly string StagePath = "res://stage/stage_{0:D4}.tscn";

    [Export]
    public bool PauseAutoScroll { get; set; } = false;

    [ExportGroup("BGM")]

    /// <summary>
    /// BGMなし
    /// </summary>
    [Export]
    public bool NoBgm { get; set; }

    [Export]
    public AudioStream BgmStream { get; set; }

    private PathFollow _autoScroll;

    public override void _Ready()
    {
        GetNode<CharacterManager>("CharacterManager").EntryCharacterNodes();
        Camera camera = GetNode<Camera>("%Camera");
        _autoScroll = GetNode<PathFollow>("AutoScrollPath/AutoScroll");
        camera.Enabled = true;
    }

    public override void InitializeNode()
    {
        GetNode<GameDataManager>("/root/GameDataManager").Restore();
        GetTree().CallGroup(IGameNode.GameNodeGroup, "InitializeNode");
        LoadState();
        PlayBgm();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!PauseAutoScroll)
        {
            _autoScroll.ManualScroll(delta);
        }
    }

    public static string GetResourcePath(StageData stageData) => string.Format(StagePath, stageData.StageNo);

    public void AddScene(Node node, string parentNodeName)
    {
        if (GetNodeOrNull(parentNodeName) is Node parentNode)
        {
            AddSceneToNode(node, parentNode);
        }
    }

    public void AddSceneToNode(Node node, Node parentNode) => CallDeferred(MethodName.DeferredAddSceneToNode, [node, parentNode]);

    private void DeferredAddSceneToNode(Node node, Node parentNode)
    {
        parentNode.AddChild(node);
        InitializeNodeAll(node);
        ActiveAllCharacter(node, GetNode<CharacterManager>("CharacterManager"));
    }

    private static void InitializeNodeAll(Node root)
    {
        if (root is null)
        {
            return;
        }

        if (root is IGameNode inode)
        {
            inode.InitializeNode();
        }

        foreach (Node n in root.GetChildren())
        {
            InitializeNodeAll(n);
        }
    }

    private static void ActiveAllCharacter(Node root, CharacterManager characterManager)
    {
        if (root is null)
        {
            return;
        }

        if (root is ICharacterManager inode)
        {
            inode.SetCharacterManager(characterManager);
            inode.ActiveCharacter(true);
        }

        foreach (Node n in root.GetChildren())
        {
            ActiveAllCharacter(n, characterManager);
        }
    }

    public void ReparentNode(Node2D node, string nodeName) => node?.CallDeferred(Node.MethodName.Reparent, [GetNode(nodeName)]);

    protected void PlayBgm()
    {
        if (NoBgm)
        {
            GetNode<MusicPlayer>("/root/MusicPlayer").Play(MusicPlayer.Command.Mute);
            return;
        }

        if (BgmStream is null)
        {
            return;
        }

        GetNode<MusicPlayer>("/root/MusicPlayer").Play(MusicPlayer.Command.FastPlay, BgmStream);
    }

    #region IStatefulインタフェース
    /// <summary>
    /// ステージ状態の保存を行う。
    /// 画面切り替え前、セーブ前に行われる
    /// </summary>
    public void SaveState() => GetTree().CallGroup(IStateful.StatefulGroup, "StateSave");

    public void LoadState() => GetTree().CallGroup(IStateful.StatefulGroup, "StateLoad");
    #endregion
}
