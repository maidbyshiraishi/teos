using Godot;
using teos.game.weapon;

namespace teos.game.mob.enemy;

/// <summary>
/// 敵シンプル1
/// </summary>
public partial class EnemySimple1 : EnemyRoot
{
    [Export]
    public float SearchWaitTime { get; set; } = 1f;

    [Export]
    public float ClearWaitTime { get; set; } = 3f;

    private Vector2 _targetPosition = Vector2.Inf;

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

        base._Ready();
    }

    public override void _Process(double delta)
    {
        if (m_StateMachine.GetCurrentNode() == "idle")
        {
            if (m_MountPoint.GetWeapon() is WeaponRoot weapon)
            {
                if (WeaponRotationEnabled && _targetPosition != Vector2.Inf)
                {
                    m_MountPoint.RotateWeapon(delta, (_targetPosition - GlobalPosition).Angle());
                }

                weapon.Update(m_Trigger);
            }

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

    public void SearchTarget()
    {
        _targetPosition = m_Player.GlobalPosition;
    }

    public void ClearTarget()
    {
        _targetPosition = Vector2.Inf;
    }

    #region ICharacterManagerインタフェース
    public override void InitializeCharacter()
    {
        base.InitializeCharacter();
        SearchTarget();
        GetNodeOrNull<Timer>("SearchTargetTimer")?.Start();
        GetNodeOrNull<Timer>("ClearTargetTimer")?.Start();
    }
    #endregion
}
