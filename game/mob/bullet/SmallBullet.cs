namespace teos.game.mob.bullet;

/// <summary>
/// 小弾
/// </summary>
public partial class SmallBullet : BulletRoot
{
    private float _speed = 0f;

    public override void _Process(double delta)
    {
        _speed = HomingFunction.MoveToward(this, _speed, m_MaxSpeed, m_Acceleration, delta);

        if (m_AMomentWaited && RemoveScreenExited && m_OnScreen is not null && !m_OnScreen.IsOnScreen())
        {
            ExitScreen();
        }
    }
}
