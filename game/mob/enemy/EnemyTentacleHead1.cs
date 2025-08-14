using Godot;
using teos.common.system;
using teos.game.mob.tentacle;
using teos.game.weapon;

namespace teos.game.mob.enemy;

/// <summary>
/// 触手敵1
/// </summary>
public partial class EnemyTentacleHead1 : EnemyRoot
{
    [Export]
    public float SearchWaitTime { get; set; } = 1f;

    [Export]
    public float ClearWaitTime { get; set; } = 3f;

    [Export]
    public float RandomErrorX { get; set; } = 50f;

    [Export]
    public float RandomErrorY { get; set; } = 50f;

    [Export]
    public float LerpAngle { get; set; } = 10f;

    private TentacleHead _tentacleHead;
    private HomingFunction _homing;

    public override void _Ready()
    {
        if (GetNodeOrNull("SearchTargetTimer") is Timer timer1)
        {
            _ = timer1.Connect(Timer.SignalName.Timeout, new(this, MethodName.SearchTarget));
            timer1.WaitTime = SearchWaitTime;
        }

        if (GetNodeOrNull("ClearTargetTimer") is Timer timer2)
        {
            _ = timer2.Connect(Timer.SignalName.Timeout, new(this, MethodName.ClearTarget));
            timer2.WaitTime = ClearWaitTime;
        }

        if (GetParent() is TentacleHead head)
        {
            _tentacleHead = head;
        }

        _homing = new(_tentacleHead, LerpAngle);
        base._Ready();
    }

    public override void _Process(double delta)
    {
        if (m_StateMachine.GetCurrentNode() == "idle")
        {
            if (m_MountPoint.GetWeapon() is WeaponRoot weapon)
            {
                if (WeaponRotationEnabled)
                {
                    m_MountPoint.RotateWeapon(delta, (m_Player.GlobalPosition - GlobalPosition).Angle());
                }

                weapon.Update(m_Trigger);
            }

            if (_tentacleHead is not null)
            {
                _homing.Homing(delta, m_MaxSpeed, m_Acceleration);
                _tentacleHead.TrimLength();
            }

            m_OldPosition = GlobalPosition;
        }
    }

    public void SearchTarget()
    {
        RandomNumberGenerator random = new();
        _homing.SetTarget(m_Player.GlobalPosition + new Vector2(random.RandfRange(-RandomErrorX, RandomErrorX), random.RandfRange(-RandomErrorY, RandomErrorY)));
        _homing.StartHoming();
    }

    public void ClearTarget()
    {
        _homing.ClearTarget();
    }

    #region ICharacterManagerインタフェース
    public override void InitializeCharacter()
    {
        base.InitializeCharacter();
        AddToGroup(GameSpeedManager.GameSpeedManageGroup);
        SearchTarget();
        GetNodeOrNull<Timer>("SearchTargetTimer")?.Start();
        GetNodeOrNull<Timer>("ClearTargetTimer")?.Start();
    }
    #endregion
}
