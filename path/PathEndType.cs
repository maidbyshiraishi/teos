namespace maid_by_shiraishi.path;

/// <summary>
/// パス終端タイプ
/// </summary>
public enum PathEndType

{
    /// <summary>
    /// 端まで行ったら終わり
    /// </summary>
    Oneway,

    /// <summary>
    /// 往復する
    /// </summary>
    Shuttle,

    /// <summary>
    /// 終端から発端へ戻る
    /// </summary>
    Loop
}
