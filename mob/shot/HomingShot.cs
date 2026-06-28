using Godot;

namespace maid_by_shiraishi.mob.shot;

/// <summary>
/// ホーミング弾
/// </summary>
public partial class HomingShot : ShotRoot
{
    [Export]
    public float LerpAngle { get; set; } = 10f;

    private HomingFunction _homing;

    public override void _Ready()
    {
        base._Ready();

        // Godotエディタからシグナルを接続すると
        // リリースビルドのエクスポート時、接続が失われることがある。
        if (GetNodeOrNull("StopHomingTimer") is Timer stopHomingTimer)
        {
            stopHomingTimer.Timeout += StopHoming;
        }

        if (GetNodeOrNull("ClearTargetTimer") is Timer clearTargetTimer)
        {
            clearTargetTimer.Timeout += ClearTarget;
        }

        if (GetNodeOrNull("FindTargetTimer") is Timer findTargetTimer)
        {
            findTargetTimer.Timeout += FindTarget;
        }

        _homing = new(this, LerpAngle);
    }

    public override void _PhysicsProcess(double delta)
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

    public void ClearTarget() => _homing.ClearTarget();
}
