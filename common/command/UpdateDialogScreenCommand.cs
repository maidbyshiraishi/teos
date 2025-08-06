using Godot;
using teos.common.system;

namespace teos.common.command;

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
