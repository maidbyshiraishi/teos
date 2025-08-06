using Godot;

namespace teos.game.tilemap.mob;

/// <summary>
/// ステージ1ボス
/// </summary>
public partial class StageBoss1 : TileMapMob
{
    private int _state = 0;
    private Timer _timer;
    private float _angle;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        base._Ready();
    }

    public override void _Process(double delta)
    {
        if (!m_Active)
        {
            return;
        }

        if (Destroyed())
        {
            Dead();
            return;
        }

        switch (_state)
        {
            case 0:

                PathFollowMove(delta);
                break;

            case 1:

                PathFollowMove(delta);
                Rotation = (float)Mathf.LerpAngle(Rotation, _angle, Mathf.DegToRad(10f) * delta);
                break;

            default:
                break;
        }
    }

    public override void AdvanceBossState(int state, int value)
    {
        _state = state;

        switch (_state)
        {
            case 0:
                break;

            case 1:

                _angle = Rotation + Mathf.DegToRad(value);
                _timer.WaitTime = 3f;
                _timer.Start();
                break;

            default:
                break;
        }
    }

    public void TimerTimeout()
    {
        switch (_state)
        {
            case 0:
                break;

            case 1:

                _state = 0;
                break;

            default:
                break;
        }
    }
}
