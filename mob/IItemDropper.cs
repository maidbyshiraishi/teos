using maid_by_shiraishi.stage.character_manager;

namespace maid_by_shiraishi.mob;

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
