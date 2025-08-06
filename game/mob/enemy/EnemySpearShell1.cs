using Godot;
using teos.game.mob.player;
using teos.game.stage.character_manager;

namespace teos.game.mob.enemy;

/// <summary>
/// ヤリ貝敵1
/// </summary>
public partial class EnemySpearShell1 : EnemyRoot
{
    [Export]
    public float LerpAngle { get; set; } = 20f;

    private HomingFunction _homing;

    public override void _Ready()
    {
        base._Ready();
        _homing = new(this, LerpAngle);
    }

    public override void _Process(double delta)
    {
        if (m_StateMachine.GetCurrentNode() == "idle")
        {
            _homing.Homing(delta, m_MaxSpeed, m_Acceleration);

            if (m_AMomentWaited && m_OnScreen is not null && !m_OnScreen.IsOnScreen())
            {
                ExitScreen();
            }
        }
    }

    public override void Burialed(Node2D node)
    {
        if (m_StateMachine.GetCurrentNode() == "idle")
        {
            m_StateMachine.Travel("stuck");
            StopHoming();
            GetNode("TentacleRoot_1").Reparent(GetParent());
            GetNode<ManualCharacterEnabler>("ManualCharacterEnabler")?.EnableCharacter();
        }
    }

    public void StopHoming()
    {
        GetNode<Timer>("FindTargetTimer").Stop();
        _homing.StopHoming();
    }

    public void FindTarget()
    {
        if (Player.GetPlayer(this) is Player player)
        {
            _homing.SetTarget(player.GlobalPosition);
            _homing.StartHoming();
        }
    }
}
