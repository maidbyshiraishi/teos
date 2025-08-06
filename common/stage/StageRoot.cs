using Godot;
using teos.common.screen;
using teos.common.system;

namespace teos.common.stage;

/// <summary>
/// ステージの親
/// </summary>
public partial class StageRoot : DialogRoot, IStateful
{
    public static readonly string ProcessGroup = "ProcessGroup";

    [ExportGroup("BGM")]

    /// <summary>
    /// BGMなし
    /// </summary>
    [Export]
    public bool NoBgm { get; set; }

    [Export]
    public AudioStream BgmStream { get; set; }

    public override void InitializeNode()
    {
        GetTree().CallGroup(IGameNode.GameNodeGroup, "InitializeNode");
        LoadState();
        PlayBgm();
    }

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
    public void SaveState()
    {
        GetTree().CallGroup(IStateful.StatefulGroup, "StateSave");
    }

    public void LoadState()
    {
        GetTree().CallGroup(IStateful.StatefulGroup, "StateLoad");
    }
    #endregion
}
