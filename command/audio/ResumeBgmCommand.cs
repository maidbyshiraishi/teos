using Godot;
using teos.system;

namespace teos.command.audio;

/// <summary>
/// BGMの一時停止を解除するコマンド
/// </summary>
public partial class ResumeBgmCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<MusicPlayer>("/root/MusicPlayer").Pause(false);
    }
}
