using Godot;
using teos.system;

namespace teos.command;

/// <summary>
/// ダイアログをすべて閉じるコマンド
/// </summary>
public partial class CloseAllDialog : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").CloseAllDialog();
    }
}
