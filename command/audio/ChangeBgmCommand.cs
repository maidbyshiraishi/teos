using Godot;
using teos.system;

namespace teos.command.audio;

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
