using Godot;

namespace teos.game.mob;

/// <summary>
/// ブロック埋没判定
/// </summary>
public partial class BurialArea : Area2D
{
    private bool _blink = false;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        _collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
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
