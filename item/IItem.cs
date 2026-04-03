using Godot;

namespace maid_by_shiraishi.item;

/// <summary>
/// アイテムのインターフェース
/// </summary>
public interface IItem
{
    #region IItemインタフェース
    void ExecItem(Area2D node);
    #endregion
}
