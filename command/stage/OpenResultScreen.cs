using Godot;
using teos.data;
using teos.system;

namespace teos.command.stage;

/// <summary>
/// 結果発表画面を表示するコマンド
/// </summary>
public partial class OpenResultScreen : CommandRoot
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
    public bool Ending { get; set; } = false;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        PlayerData playerData = GetNode<GameDataManager>("/root/GameDataManager").GetPlayerData();
        int score = playerData.Score;
        int rank = GetNode<HighScoreManager>("/root/HighScoreManager").EntryHighScore(score);
        GetNode<DialogLayer>("/root/DialogLayer").OpenScreen(Ending ? "res://screen/ending_result_screen.tscn" : "res://screen/result_screen.tscn", Fadeout, Fadein, "ResultScreen", [rank, score]);
    }
}
