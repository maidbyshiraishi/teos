using Godot;
using maid_by_shiraishi.item;
using maid_by_shiraishi.weapon;

namespace maid_by_shiraishi.mob;

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
        // Godotエディタからシグナルを接続すると
        // リリースビルドのエクスポート時、接続が失われることがある。
        if (GetNodeOrNull("Timer") is Timer timer)
        {
            timer.Timeout += Switch;
        }

        if (GetParent() is Fighter fighter)
        {
            _target = fighter;
        }

        _collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
        AreaEntered += Area2DEntered;
    }

    public void Area2DEntered(Area2D area) => CallDeferred(MethodName.DeferredNodeEntered, [area]);

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
