using Godot;
using teos.common.screen;
using teos.game.system;

namespace teos.game.screen;

/// <summary>
/// セーブとロードダイアログ
/// </summary>
public partial class SaveAndLoadDialog : DialogRoot
{
    public override void Active()
    {
        base.Active();
        UpdateTimeStamp();
    }

    private void UpdateTimeStamp()
    {
        string[] date = GetNode<GameDataManager>("/root/GameDataManager").GetFileDates();

        for (int i = 1; i <= GameDataManager.NumOfSaveFiles; i++)
        {
            if (date[i - 1] is not null)
            {
                GetNode<Control>("Control/Load").Show();
                break;
            }
        }
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "Back";
    }
}
