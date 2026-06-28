using Godot;
using Godot.Collections;
using maid_by_shiraishi.mob.shot;
using maid_by_shiraishi.weapon;

namespace maid_by_shiraishi.mob.enemy;

/// <summary>
/// 敵ドローン1
/// </summary>
public partial class EnemyDrone1 : EnemyRoot
{
    [ExportGroup("Shot")]

    [Export]
    public PackedScene Shot { get; set; }

    private Array<Marker2D> _muzzle = [];

    public override void _Ready()
    {
        _muzzle = WeaponRoot.FindMuzzle(GetNodeOrNull("Muzzle"));
        base._Ready();

        // Godotエディタからシグナルを接続すると
        // リリースビルドのエクスポート時、接続が失われることがある。
        if (GetNodeOrNull("ShotTimer") is Timer timer)
        {
            timer.Timeout += Fire;
        }
    }

    public override void _PhysicsProcess(double delta)
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
        if (!m_Trigger || m_StateMachine.GetCurrentNode() != "idle" || Shot is null || Shot.Instantiate() is not ShotRoot)
        {
            return;
        }

        foreach (Marker2D marker in _muzzle)
        {
            MakeShot(marker, Shot.Instantiate() as ShotRoot);
        }
    }

    private void MakeShot(Marker2D maker, ShotRoot shot)
    {
        PlaySe("gun_shot");
        shot.Transform = maker.GlobalTransform;
        shot.CollisionMask = ShotTargetLayer;
        shot.ShotModulate = ShotModulate;
        shot.AddToGroup(EnemyGroup);
        _ = EmitSignal(Mob.SignalName.SceneAdded, [shot, ShotRoot.ParentNodeName]);
    }
}
