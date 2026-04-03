using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.option;

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
