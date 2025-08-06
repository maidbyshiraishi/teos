using teos.game.mob;
using teos.game.mob.bullet;

namespace teos.game.weapon;

/// <summary>
/// 武器(剣と盾)
/// </summary>
public partial class Sword : WeaponRoot
{
    public override void Fire()
    {
        if (NumOfBullets > 0)
        {
            NumOfBullets--;
        }
    }

    public override bool Equip(Fighter fighter, MountPoint mountPoint, bool enemy, bool instantly)
    {
        bool ret = base.Equip(fighter, mountPoint, enemy, instantly);

        if (ret)
        {
            BulletRoot bullet = GetNode<BulletRoot>("Blade");
            bullet.CollisionMask = m_BulletTargetLayer;
            bullet.BulletModulate = m_BulletModulate;
        }

        return ret;
    }
}
