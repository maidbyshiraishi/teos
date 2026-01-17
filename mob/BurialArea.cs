using Godot;

namespace teos.mob;

/// <summary>
/// ブロック埋没判定
/// </summary>
public partial class BurialArea : Area2D
{
    private bool _blink = false;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        // Godotエディタからシグナルを接続すると
        // リリースビルドのエクスポート時、接続が失われることがある。
        _ = GetNodeOrNull<Timer>("Timer")?.Connect(Timer.SignalName.Timeout, new(this, MethodName.Switch));

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
