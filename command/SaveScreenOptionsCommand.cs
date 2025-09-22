using Godot;
using teos.system;

namespace teos.command;

/// <summary>
/// 画面オプションを保存するコマンド
/// </summary>
public partial class SaveScreenOptionsCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<ScreenOption>("/root/ScreenOption").SaveScreenOptions();
    }
}
