using Godot;
using teos.common.stage;

namespace teos.game.weapon;

/// <summary>
/// 武器(回転ガン)
/// </summary>
public partial class WheelGun : WeaponRoot
{
    [Export]
    public float Angle { get; set; } = 5f;

    public override void _Ready()
    {
        base._Ready();
        AddToGroup(StageRoot.ProcessGroup);
    }

    public override void _Process(double delta)
    {
        if (m_StateMachine.GetCurrentNode() == "idle" || m_StateMachine.GetCurrentNode() == "attack" || m_StateMachine.GetCurrentNode() == "enemy_idle" || m_StateMachine.GetCurrentNode() == "enemy_attack")
        {
            Rotate(Angle * (float)delta);
        }
    }
}
