using Godot;
using teos.game.mob;
using teos.game.system;

namespace teos.game.weapon;

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
            GetNode<GameDialogLayer>("/root/DialogLayer").GetCurrentGameRoot().ReparentNode(_weapon, "Item");
            _weapon = null;
        }
    }

    public WeaponRoot GetWeapon()
    {
        return _weapon;
    }

    public void RotateWeapon(double delta, float angle)
    {
        if (_weapon is not null && _weapon.RotationEnabled)
        {
            GlobalRotation = (float)Mathf.LerpAngle(GlobalRotation, angle, _lerpAngle * delta);
        }
    }
}
