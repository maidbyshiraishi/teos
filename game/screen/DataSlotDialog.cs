using Godot;
using teos.common.screen;
using teos.game.system;

namespace teos.game.screen;

/// <summary>
/// データスロット系ダイアログ(セーブダイアログとロードダイアログ)
/// </summary>
public partial class DataSlotDialog : DialogRoot
{
    /// <summary>
    /// ロード系か
    /// ロード系でなければセーブ系
    /// </summary>
    [Export]
    public bool LoadMode { get; set; }

    public override void Active()
    {
        UpdateTimeStamp();
        base.Active();
    }

    private void UpdateTimeStamp()
    {
        string[] date = GetNode<GameDataManager>("/root/GameDataManager").GetFileDates();

        for (int i = 1; i <= GameDataManager.NumOfSaveFiles; i++)
        {
            Button b = GetNode<Button>($"Control/Data_{i}");
            Label l = GetNode<Label>($"Date_{i}");

            if (date[i - 1] is null)
            {
                b.Disabled = LoadMode;
                l.Text = "新規";
                continue;
            }

            b.Disabled = false;
            l.Text = date[i - 1];

            string fileThumbnail = string.Format(GameDataManager.DataThumbnailPath, i);

            if (!FileAccess.FileExists(fileThumbnail))
            {
                continue;
            }

            GetNode<Sprite2D>($"Sprite2D_{i}").Texture = ImageTexture.CreateFromImage(Image.LoadFromFile(fileThumbnail));
        }
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "Back";
    }
}
