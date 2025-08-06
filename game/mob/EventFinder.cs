using Godot;
using teos.game.item;
using teos.game.weapon;

namespace teos.game.mob;

/// <summary>
/// イベント処理との接触判定を行う。
/// </summary>
public partial class EventFinder : Area2D
{
    private Area2D _target;
    private bool _blink = false;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        if (GetParent() is Fighter fighter)
        {
            _target = fighter;
        }

        _collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
        _ = Connect(Area2D.SignalName.AreaEntered, new(this, MethodName.Area2DEntered));
    }

    public void Area2DEntered(Area2D area)
    {
        _ = CallDeferred(MethodName.DeferredNodeEntered, [area]);
    }

    public void DeferredNodeEntered(Area2D node)
    {
        if (node is IItem item)
        {
            item.ExecItem(_target);
        }
        else if (node is WeaponRoot weapon && _target is Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
        }
    }

    public void Switch()
    {
        if (_collisionShape is not null)
        {
            _blink = !_blink;
            _collisionShape.Disabled = _blink;
        }
    }
}
