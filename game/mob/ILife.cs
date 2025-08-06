namespace teos.game.mob;

/// <summary>
/// ライフ操作のインターフェース
/// </summary>
public interface ILife
{
    #region ILifeインタフェース
    void AddLife(int value) { }

    void FullRecovered() { }

    void Recovered() { }

    void Damaged() { }

    void Dead() { }
    #endregion
}
