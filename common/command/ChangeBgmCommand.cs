using Godot;
using teos.common.system;

namespace teos.common.command;

/// <summary>
/// BGMを変更するコマンド
/// </summary>
public partial class ChangeBgmCommand : CommandRoot
{
    /// <summary>
    /// MusicPlayerコマンド
    /// </summary>
    [Export]
    public MusicPlayer.Command Command { get; set; }

    /// <summary>
    /// 対象とするオーディオストリーム
    /// </summary>
    [Export]
    public AudioStream Stream { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<MusicPlayer>("/root/MusicPlayer").Play(Command, Stream);
    }
}
