namespace maid_by_shiraishi.path;

/// <summary>
/// パス移動時に終端処理を行うインタフェース
/// </summary>
public interface IPathFollower
{
    #region IPathFollowerインタフェース
    void ReachedToEdge(float progressRatio, bool reverse, PathEndType edgeHandling) { }
    #endregion
}
