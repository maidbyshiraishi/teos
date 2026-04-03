using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

/// <summary>
/// ダイアログを閉じるコマンド
/// </summary>
public partial class CloseDialogCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").CloseDialog();
    }
}
