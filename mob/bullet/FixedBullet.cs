using Godot;

namespace maid_by_shiraishi.mob.bullet;

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
        if (GetNodeOrNull("Timer") is Timer timer)
        {
            timer.Timeout += Switch;
        }
    }
}
