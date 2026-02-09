using Godot;

namespace teos.mob.bullet;

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

        // Godotエディタからシグナルを接続すると
        // リリースビルドのエクスポート時、接続が失われることがある。
        _ = GetNodeOrNull<Timer>("StopHomingTimer")?.Connect(Timer.SignalName.Timeout, new(this, MethodName.StopHoming));
        _ = GetNodeOrNull<Timer>("ClearTargetTimer")?.Connect(Timer.SignalName.Timeout, new(this, MethodName.ClearTarget));
        _ = GetNodeOrNull<Timer>("FindTargetTimer")?.Connect(Timer.SignalName.Timeout, new(this, MethodName.FindTarget));

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

    public void ClearTarget() => _homing.ClearTarget();
}
