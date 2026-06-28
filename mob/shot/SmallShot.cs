namespace maid_by_shiraishi.mob.shot;

/// <summary>
/// 小弾
/// </summary>
public partial class SmallShot : ShotRoot
{
    private float _speed = 0f;

    public override void _PhysicsProcess(double delta)
    {
        _speed = HomingFunction.MoveToward(this, _speed, m_MaxSpeed, m_Acceleration, delta);

        if (m_AMomentWaited && RemoveScreenExited && m_OnScreen is not null && !m_OnScreen.IsOnScreen())
        {
            ExitScreen();
        }
    }
}
