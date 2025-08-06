namespace teos.common.stage;

/// <summary>
/// 画面間で状態を保存するオブジェクトのインタフェース
/// </summary>
public interface IStateful
{
    static readonly string StatefulGroup = "StatefulGroup";

    #region IStatefulインタフェース
    void SaveState();

    void LoadState();
    #endregion
}
