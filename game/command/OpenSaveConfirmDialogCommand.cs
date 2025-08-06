using Godot;
using teos.common.command;
using teos.common.system;

namespace teos.game.command;

/// <summary>
/// セーブ確認ダイアログを開くコマンド
/// </summary>
public partial class OpenSaveConfirmDialogCommand : CommandRoot
{
    /// <summary>
    /// セーブデータ番号
    /// </summary>
    [Export]
    public int SlotNo { get; set; } = 0;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://game/screen/save_confirm_dialog.tscn", "SaveConfirmDialog", [SlotNo]);
    }
}
