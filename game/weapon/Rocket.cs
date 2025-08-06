using Godot;
using teos.game.mob;
using teos.game.mob.bullet;

namespace teos.game.weapon;

/// <summary>
/// 武器(ロケット)
/// </summary>
public partial class Rocket : WeaponRoot
{
    [ExportGroup("Burning Speed")]

    [Export]
    public float BurningApproach { get; set; } = 256f;

    /// <summary>
    /// 最大速度
    /// </summary>
    [Export]
    public float BurningMaxSpeed { get; set; } = 700f;

    /// <summary>
    /// 減速に要する距離
    /// </summary>
    [Export]
    public float BurningReductionApproach { get; set; } = 256f;

    [Export]
    public float BurningAutoScrollSpeed { get; set; } = 300f;

    private Fighter _fighter;
    private bool _fire = false;

    public override void Fire()
    {
        if (NumOfBullets > 0)
        {
            if (!_fire)
            {
                _fighter?.CalcSpeed(BurningMaxSpeed, BurningApproach, BurningReductionApproach);
                _fighter?.UpdateAutoScrollSpeed(BurningAutoScrollSpeed);
            }

            _fire = true;
            NumOfBullets--;
        }
    }

    public void Idle()
    {
        if (_fire)
        {
            CalcInitialSpeed();
        }

        _fire = false;
    }

    public void CalcInitialSpeed()
    {
        // ステートマシンからも呼ばれる
        _fighter?.CalcSpeed(MaxSpeed, Approach, ReductionApproach);
        _fighter?.UpdateAutoScrollSpeed(AutoScrollSpeed);
    }

    public override bool Equip(Fighter fighter, MountPoint mountPoint, bool enemy, bool instantly)
    {
        bool ret = base.Equip(fighter, mountPoint, enemy, instantly);
        _fighter = null;

        if (ret)
        {
            BulletRoot bullet = GetNode<BulletRoot>("Blade");
            bullet.CollisionMask = m_BulletTargetLayer;
            bullet.BulletModulate = m_BulletModulate;
            _fighter = fighter;
        }

        return ret;
    }
}
