using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.audio;

/// <summary>
/// BGMを一時停止するコマンド
/// </summary>
public partial class PauseBgmCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<MusicPlayer>("/root/MusicPlayer").Pause(true);
    }
}
