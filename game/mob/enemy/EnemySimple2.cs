using Godot;
using Godot.Collections;
using teos.game.mob.bullet;
using teos.game.weapon;

namespace teos.game.mob.enemy;

/// <summary>
/// 敵シンプル2
/// </summary>
public partial class EnemySimple2 : EnemyRoot
{
    [ExportGroup("Bullet")]

    [Export]
    public PackedScene Bullet { get; set; }

    private Array<Marker2D> _muzzle = [];

    public override void _Ready()
    {
        _muzzle = WeaponRoot.FindMuzzle(GetNodeOrNull("Muzzle"));
        base._Ready();
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
        if (Bullet is null || Bullet.Instantiate() is not BulletRoot)
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
