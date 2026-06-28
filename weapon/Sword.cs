using maid_by_shiraishi.mob;
using maid_by_shiraishi.mob.shot;

namespace maid_by_shiraishi.weapon;

/// <summary>
/// 武器(剣と盾)
/// </summary>
public partial class Sword : WeaponRoot
{
    public override void Fire()
    {
        if (NumOfShots > 0)
        {
            NumOfShots--;
        }
    }

    public override bool Equip(Fighter fighter, MountPoint mountPoint, bool enemy, bool instantly)
    {
        bool ret = base.Equip(fighter, mountPoint, enemy, instantly);

        if (ret)
        {
            ShotRoot shot = GetNode<ShotRoot>("Blade");
            shot.CollisionMask = m_ShotTargetLayer;
            shot.ShotModulate = m_ShotModulate;
        }

        return ret;
    }
}
