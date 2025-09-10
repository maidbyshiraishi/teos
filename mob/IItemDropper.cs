using teos.stage.character_manager;

namespace teos.mob;

/// <summary>
/// アイテムを落とすインターフェース
/// </summary>
public interface IItemDropper
{
    #region IItemDropperインタフェース
    void AddItemDropper(EnemyDropCharacterEnabler enabler);

    void DropItem();
    #endregion
}
