using Godot;
using Godot.Collections;
using teos.mob.bullet;
using teos.weapon;

namespace teos.mob.enemy;

/// <summary>
/// 敵ドローン1
/// </summary>
public partial class EnemyDrone1 : EnemyRoot
{
    [ExportGroup("Bullet")]

    [Export]
    public PackedScene Bullet { get; set; }

    private Array<Marker2D> _muzzle = [];

    public override void _Ready()
    {
        _muzzle = WeaponRoot.FindMuzzle(GetNodeOrNull("Muzzle"));
        base._Ready();

        // Godotエディタからシグナルを接続すると
        // リリースビルドのエクスポート時、接続が失われることがある。
        _ = GetNodeOrNull<Timer>("ShotTimer")?.Connect(Timer.SignalName.Timeout, new(this, MethodName.Fire));
    }

    public override void _Process(double delta)
    {
        if (m_StateMachine.GetCurrentNode() == "idle")
        {
            PathFollowMove(m_PathFollow, delta);

            if (m_Sprite2d is not null)
            {
                if (GlobalPosition.X < m_OldPosition.X)
                {
                    m_Sprite2d.FlipH = false;
                }
                else if (m_OldPosition.X < GlobalPosition.X)
                {
                    m_Sprite2d.FlipH = true;
                }
            }

            m_OldPosition = GlobalPosition;
        }
    }

    public virtual void Fire()
    {
        if (!m_Trigger || m_StateMachine.GetCurrentNode() != "idle" || Bullet is null || Bullet.Instantiate() is not BulletRoot)
        {
            return;
        }

        foreach (Marker2D marker in _muzzle)
        {
            MakeBullet(marker, Bullet.Instantiate() as BulletRoot);
        }
    }

    private void MakeBullet(Marker2D maker, BulletRoot bullet)
    {
        PlaySe("gun_shot");
        bullet.Transform = maker.GlobalTransform;
        bullet.CollisionMask = BulletTargetLayer;
        bullet.BulletModulate = BulletModulate;
        bullet.AddToGroup(EnemyGroup);
        _ = EmitSignal(Mob.SignalName.SceneAdded, [bullet, BulletRoot.ParentNodeName]);
    }
}
