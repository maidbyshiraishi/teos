using Godot;
using teos.common.screen;
using teos.common.system;
using teos.game.system;

namespace teos.game.screen;

/// <summary>
/// セーブ確認ダイアログ
/// </summary>
public partial class SaveConfirmDialog : DialogRoot
{
    private int _slotNo;

    public override void GetArgument()
    {
        GetDialogArgument("SaveConfirmDialog");

        if (m_Argument is null || m_Argument.Count != 1 || m_Argument[0].VariantType is not Variant.Type.Int)
        {
            return;
        }

        _slotNo = (int)m_Argument[0];
        GetNode<Label>("Data").Text = $"データ{_slotNo}";
        string date = GetNode<GameDataManager>("/root/GameDataManager").GetFileDate(_slotNo);

        if (date is null)
        {
            GetNode<Label>("Date").Text = "新規";
        }
        else
        {
            GetNode<Label>("Date").Text = date;
            string fileThumbnail = string.Format(GameDataManager.DataThumbnailPath, _slotNo);

            if (FileAccess.FileExists(fileThumbnail))
            {
                GetNode<Sprite2D>($"Sprite2D").Texture = ImageTexture.CreateFromImage(Image.LoadFromFile(fileThumbnail));
            }
        }
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "No";
    }

    /// <summary>
    /// はい
    /// </summary>
    public void Yes()
    {
        if (_slotNo < 0 || GameDataManager.NumOfSaveFiles < _slotNo)
        {
            string msg = $"ゲームデータのセーブ中にエラーが発生しました。エラーの発生したデータはゲームデータ{_slotNo}です。";
            GD.PrintErr(msg);
            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://common/screen/error_dialog.tscn", "ErrorDialog", [msg]);
            return;
        }

        Error e = GetNode<GameDataManager>("/root/GameDataManager").Save(_slotNo);

        if (e is not Error.Ok)
        {
            string msg = $"ゲームデータのセーブ中にエラーが発生しました。エラーの発生したデータはゲームデータ{_slotNo}です。エラーの値は{e}です。";
            GD.PrintErr(msg);
            GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://common/screen/error_dialog.tscn", "ErrorDialog", [msg]);
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").CloseDialog();
    }
}
