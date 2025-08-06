using Godot;
using teos.common.command;
using teos.game.weapon;

namespace teos.game.mob;

/// <summary>
/// 主人公と敵の親
/// </summary>
public partial class Fighter : Mob
{
    [Export]
    public bool AffectedByWeapon { get; set; } = false;

    [Export]
    public Vector2 DefaultDirection { get; set; } = Vector2.Right;

    [Export]
    public bool PlaySeEquipWeapon { get; set; } = false;

    [ExportGroup("Bullet")]

    [Export(PropertyHint.Layers2DPhysics)]
    public uint BulletTargetLayer { get; set; }

    [Export]
    public Color BulletModulate { get; set; } = Color.Color8(255, 255, 255);

    public override void _Ready()
    {
        _ = GetNodeOrNull<EventFinder>("EventFinder")?.Connect(Area2D.SignalName.AreaEntered, new(this, MethodName.EventOccurred));
        _ = GetNodeOrNull<BurialArea>("BurialArea")?.Connect(Area2D.SignalName.BodyEntered, new(this, MethodName.Burialed));
        base._Ready();
    }

    public virtual void Burialed(Node2D node)
    {
    }

    public virtual void EventOccurred(Area2D node)
    {
    }

    public virtual void EquipWeapon(WeaponRoot weapon, bool instantly = false)
    {
        CommandRoot.ExecChildren(GetNodeOrNull("EquipWeapon"), this, true);
    }

    public virtual void SeparateWeapon()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("SeparateWeapon"), this, true);
    }

    public override void CalcSpeed(float speed, float approach, float reductionApproach)
    {
        if (AffectedByWeapon)
        {
            base.CalcSpeed(speed, approach, reductionApproach);
        }
    }

    public virtual void UpdateAutoScrollSpeed(float? speed)
    {
    }
}
