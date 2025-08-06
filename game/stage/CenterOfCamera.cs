using Godot;

namespace teos.game.stage;

/// <summary>
/// 画面中央に配置され、CenterOfCameraCharacterEnablerと接触する
/// </summary>
public partial class CenterOfCamera : Area2D
{
    private Vector2 _oldPosition;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        _oldPosition = GlobalPosition;
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public override void _Process(double delta)
    {
        Vector2 vec = GlobalPosition - _oldPosition;
        _collisionShape.Position = vec.Length() > 2.0f ? vec : Vector2.Zero;
        _oldPosition = GlobalPosition;
    }
}
