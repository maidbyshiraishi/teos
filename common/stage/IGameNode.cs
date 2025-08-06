namespace teos.common.stage;

/// <summary>
/// ゲーム内で使用されるオブジェクト全般のインタフェース
/// </summary>
public interface IGameNode
{
    static readonly string GameNodeGroup = "GameNode";

    #region IGameNodeインタフェース
    /// <summary>
    /// 初期化
    /// </summary>
    void InitializeNode() { }

    /// <summary>
    /// 除去
    /// </summary>
    void RemoveNode() { }
    #endregion
}
