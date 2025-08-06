using Godot;
using teos.common.system;
using teos.game.stage;

namespace teos.game.system;

/// <summary>
/// 画面遷移・ダイアログ制御
/// DialogLayerに対して、ゲームタイトル固有の拡張を行う。
/// プロジェクト設定＞グローバル＞自動読み込みで自動的に実行が開始される。
/// </summary>
public partial class GameDialogLayer : DialogLayer
{
    /// <summary>
    /// ゲーム画面を開く
    /// </summary>
    /// <param name="startStageType">ゲーム開始種別</param>
    /// <param name="slotNo">データ番号</param>
    public void OpenGame(StartGameType startStageType, int slotNo, string fadeout, string fadein)
    {
        GetTree().Paused = true;
        _ = CallDeferred(MethodName.DeferredOpenGame, [(int)startStageType, slotNo, fadeout, fadein]);
    }

    private void DeferredOpenGame(StartGameType startStageType, int slotNo, string fadeout, string fadein)
    {
        GameDataManager gameDataManager = GetNode<GameDataManager>("/root/GameDataManager");
        Error e = Error.Ok;

        // TravelStageとRestartは何もしない。
        switch (startStageType)
        {
            case StartGameType.NewGame:

                e = gameDataManager.LoadInitialStartData();
                break;

            case StartGameType.Load:

                e = gameDataManager.Load(slotNo);
                break;
        }

        if (e is not Error.Ok)
        {
            string msg = $"ゲームを開始できません。エラーの値は{e}です。";
            GD.PrintErr(msg);
            return;
        }

        string path = GameStageRoot.GetResourcePath(gameDataManager.GetStageData());

        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PrintErr("pathがnullまたはホワイトスペースです。ChangeSceneToFile()できません。");
            return;
        }

        DeferredOpenScreen(path, fadeout, fadein);
    }

    public GameStageRoot GetCurrentGameRoot()
    {
        return GetCurrentStageRoot() is GameStageRoot gameStageRoot ? gameStageRoot : null;
    }
}
