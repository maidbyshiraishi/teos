using Godot;

namespace teos.game.item;

/// <summary>
/// アイテムのインターフェース
/// </summary>
public interface IItem
{
    #region IItemインタフェース
    void ExecItem(Area2D node);
    #endregion
}
