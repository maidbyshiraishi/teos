using Godot;
using maid_by_shiraishi.stage;

namespace maid_by_shiraishi.weapon;

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
        AddToGroup(GameStageRoot.ProcessGroup);
    }

    public override void _Process(double delta)
    {
        if (m_StateMachine.GetCurrentNode() == "idle" || m_StateMachine.GetCurrentNode() == "attack" || m_StateMachine.GetCurrentNode() == "enemy_idle" || m_StateMachine.GetCurrentNode() == "enemy_attack")
        {
            Rotate(Angle * (float)delta);
        }
    }
}
