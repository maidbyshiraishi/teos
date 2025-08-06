using Godot;
using teos.common.system;

namespace teos.common.command;

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
