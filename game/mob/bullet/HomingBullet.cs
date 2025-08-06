using Godot;

namespace teos.game.mob.bullet;

/// <summary>
/// ホーミング弾
/// </summary>
public partial class HomingBullet : BulletRoot
{
    [Export]
    public float LerpAngle { get; set; } = 10f;

    private HomingFunction _homing;

    public override void _Ready()
    {
        base._Ready();
        _homing = new(this, LerpAngle);
    }

    public override void _Process(double delta)
    {
        _homing.Homing(delta, m_MaxSpeed, m_Acceleration);

        if (m_AMomentWaited && RemoveScreenExited && m_OnScreen is not null && !m_OnScreen.IsOnScreen())
        {
            ExitScreen();
        }
    }

    public void StopHoming()
    {
        GetNode<Timer>("FindTargetTimer").Stop();
        _homing.StopHoming();
    }

    public void FindTarget()
    {
        // 4:Player, 6:Enemy
        if (_homing.FindTarget(GetCollisionMaskValue(6), GetCollisionMaskValue(4)) is Mob mob)
        {
            _homing.SetTarget(mob.GlobalPosition);
            _homing.StartHoming();
        }
    }

    public void ClearTarget()
    {
        _homing.ClearTarget();
    }
}
