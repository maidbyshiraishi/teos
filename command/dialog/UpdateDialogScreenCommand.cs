using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

/// <summary>
/// ダイアログを再描画するコマンド
/// </summary>
public partial class UpdateDialogScreenCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").UpdateDialogScreen();
    }
}
