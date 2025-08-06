using Godot;
using teos.common.command;
using teos.common.system;
using teos.game.stage;
using teos.game.system;

namespace teos.game.command;

/// <summary>
/// ゲームを開始するコマンド
/// </summary>
public partial class OpenGameCommand : CommandRoot
{
    /// <summary>
    /// フェードアウトエフェクト
    /// </summary>
    [Export]
    public string Fadeout { get; set; } = "fadeout_1";

    /// <summary>
    /// フェードインエフェクト
    /// </summary>
    [Export]
    public string Fadein { get; set; } = "fadein_1";

    /// <summary>
    /// ゲーム開始種別
    /// </summary>
    [Export]
    public StartGameType StartGame { get; set; } = StartGameType.NewGame;

    /// <summary>
    /// ゲームデータ番号
    /// </summary>
    [Export]
    public int SlotNo { get; set; } = GameDataManager.NullSlotNo;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        if (StartGame is StartGameType.Load)
        {
            if (SlotNo < 0 || GameDataManager.NumOfSaveFiles < SlotNo)
            {
                string msg = $"ゲームデータのロード中にエラーが発生しました。エラーの発生したデータはゲームデータ{SlotNo}です。";
                GD.PrintErr(msg);
                GetNode<DialogLayer>("/root/DialogLayer").OpenDialog("res://common/screen/error_dialog.tscn", "ErrorDialog", [msg]);
                return;
            }
        }
        else if (StartGame is StartGameType.NewGame or StartGameType.TravelStage or StartGameType.Restart)
        {
            SlotNo = GameDataManager.NullSlotNo;
        }

        GetNode<GameDialogLayer>("/root/DialogLayer").OpenGame(StartGame, SlotNo, Fadeout, Fadein);
    }
}
