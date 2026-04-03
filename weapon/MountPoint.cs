using Godot;
using maid_by_shiraishi.mob;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.weapon;

/// <summary>
/// 銃座
/// </summary>
public partial class MountPoint : Marker2D
{
    [Export]
    public float LerpAngle
    {
        get => _lerpAngle;
        set => _lerpAngle = Mathf.DegToRad(value);
    }

    private WeaponRoot _weapon;
    private float _lerpAngle;

    public bool EquipWeapon(Fighter fighter, WeaponRoot weapon, bool enemy, bool instantly)
    {
        if (_weapon is null && weapon.Equip(fighter, this, enemy, instantly))
        {
            _weapon = weapon;
            return true;
        }

        return false;
    }

    public void SeparateWeapon(Fighter fighter)
    {
        if (_weapon is not null)
        {
            _weapon.Separate(fighter, this);
            GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot().ReparentNode(_weapon, "Item");
            _weapon = null;
        }
    }

    public WeaponRoot GetWeapon() => _weapon;

    public void RotateWeapon(double delta, float angle)
    {
        if (_weapon is not null && _weapon.RotationEnabled)
        {
            GlobalRotation = (float)Mathf.LerpAngle(GlobalRotation, angle, _lerpAngle * delta);
        }
    }
}
