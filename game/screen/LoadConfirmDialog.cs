using Godot;
using teos.common.screen;
using teos.game.command;
using teos.game.system;

namespace teos.game.screen;

/// <summary>
/// ロード確認ダイアログ
/// </summary>
public partial class LoadConfirmDialog : DialogRoot
{
    public override void GetArgument()
    {
        GetDialogArgument("LoadConfirmDialog");

        if (m_Argument is null || m_Argument.Count != 1 || m_Argument[0].VariantType is not Variant.Type.Int)
        {
            return;
        }

        int slotNo = (int)m_Argument[0];
        GetNode<Label>("Data").Text = $"データ{slotNo}";
        string date = GetNode<GameDataManager>("/root/GameDataManager").GetFileDate(slotNo);
        Button yesButton = GetNode<Button>("Control/Yes");

        if (date is null)
        {
            yesButton.Disabled = true;
            GetNode<Label>("Date").Text = "ロードできません。";
        }
        else
        {
            yesButton.Disabled = false;
            GetNode<Label>("Date").Text = date;
            GetNode<OpenGameCommand>("Control/Yes/Exec/OpenGameCommand").SlotNo = slotNo;
            string fileThumbnail = string.Format(GameDataManager.DataThumbnailPath, slotNo);

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
}
