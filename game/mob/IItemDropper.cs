using teos.game.stage.character_manager;

namespace teos.game.mob;

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
