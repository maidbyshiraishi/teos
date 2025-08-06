namespace teos.common.system.game_button_controller;

/// <summary>
/// GameButtonControllerで使用されるボタンの状態
/// </summary>
public enum GameButtonState
{
    /// <summary>
    /// 解放
    /// </summary>
    Release = 0,

    /// <summary>
    /// 長押し
    /// </summary>
    LongPress,

    /// <summary>
    /// 連打
    /// </summary>
    Repeat,

    /// <summary>
    /// 不定
    /// </summary>
    Unstable,

    Null
}
