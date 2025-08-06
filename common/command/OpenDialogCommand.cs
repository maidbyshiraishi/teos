using Godot;
using teos.common.system;

namespace teos.common.command;

/// <summary>
/// ダイアログを開くコマンド
/// </summary>
public partial class OpenDialogCommand : CommandRoot
{
    /// <summary>
    /// 開くダイアログ
    /// </summary>
    [Export]
    public string DialogPath { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").OpenDialog(DialogPath);
    }
}
