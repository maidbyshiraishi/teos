using Godot;

namespace teos.mob.bullet;

/// <summary>
/// 固定弾
/// </summary>
public partial class FixedBullet : BulletRoot
{
    public override void _Ready()
    {
        base._Ready();

        // Godotエディタからシグナルを接続すると
        // リリースビルドのエクスポート時、接続が失われることがある。
        _ = GetNodeOrNull<Timer>("Timer")?.Connect(Timer.SignalName.Timeout, new(this, BulletRoot.MethodName.Switch));
    }
}
