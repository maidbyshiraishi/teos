namespace teos.game.stage;

/// <summary>
/// ゲーム開始種別
/// </summary>
public enum StartGameType
{
    /// <summary>
    /// 最初から
    /// </summary>
    NewGame,

    /// <summary>
    /// ステージ移動
    /// </summary>
    TravelStage,

    /// <summary>
    /// ロード
    /// </summary>
    Load,

    /// <summary>
    /// ミス後の再開
    /// </summary>
    Restart
}
