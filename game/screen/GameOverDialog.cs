using Godot;
using teos.common.screen;
using teos.game.system;

namespace teos.game.screen;

/// <summary>
/// ゲームオーバーダイアログ
/// </summary>
public partial class GameOverDialog : DialogRoot
{
    public override void InitializeNode()
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
}
