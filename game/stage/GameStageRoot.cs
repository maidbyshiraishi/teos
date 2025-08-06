using Godot;
using teos.common.path;
using teos.common.stage;
using teos.game.data;
using teos.game.stage.character_manager;
using teos.game.system;

namespace teos.game.stage;

/// <summary>
/// ゲームステージの親
/// </summary>
public partial class GameStageRoot : StageRoot
{
    public static readonly string StagePath = "res://game/stage/stage_{0:D4}.tscn";

    [Export]
    public bool PauseAutoScroll { get; set; } = false;

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
        base.InitializeNode();
    }

    public override void _Process(double delta)
    {
        if (!PauseAutoScroll)
        {
            _autoScroll.ManualScroll(delta);
        }
    }

    public static string GetResourcePath(StageData stageData)
    {
        return string.Format(StagePath, stageData.StageNo);
    }

    public void AddScene(Node node, string parentNodeName)
    {
        if (GetNodeOrNull(parentNodeName) is Node parentNode)
        {
            AddSceneToNode(node, parentNode);
        }
    }

    public void AddSceneToNode(Node node, Node parentNode)
    {
        _ = CallDeferred(MethodName.DeferredAddSceneToNode, [node, parentNode]);
    }

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

    public void ReparentNode(Node2D node, string nodeName)
    {
        _ = node?.CallDeferred(Node.MethodName.Reparent, [GetNode(nodeName)]);
    }
}
