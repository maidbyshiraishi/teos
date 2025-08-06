using Godot;
using Godot.Collections;
using teos.game.command;

namespace teos.game.tilemap.mob;

/// <summary>
/// ステージ3ボス
/// </summary>
public partial class StageBoss3 : TileMapMob
{
    private int _state = 0;
    private float _angle;
    private int _pileIndex = 0;
    private Array<Marker2D> _markers = [];
    private int _index = 0;
    private AddSceneCommand _addSceneCommand;

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

    public void StartRotate()
    {
        AdvanceBossState(1, 30);
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
                break;

            default:
                break;
        }
    }

    public void MakeTentacle()
    {
        if (_markers.Count == 0)
        {
            return;
        }

        if (_markers[_index].GetChildCount() < 4)
        {
            _addSceneCommand.ParentNode = _markers[_index];
            _addSceneCommand.AddScene();
        }

        _index = Mathf.Clamp(_index + 1, 0, _markers.Count - 1);
    }

    #region ICharacterManagerインタフェース
    public override void ActiveCharacter(bool active)
    {
        m_Active = active;
        CollisionEnabled = true;
        FindWeakPoint();

        if (!active)
        {
            return;
        }

        foreach (Node n in GetChildren())
        {
            if (n is Marker2D marker)
            {
                _markers.Add(marker);
            }
        }

        _addSceneCommand = GetNodeOrNull<AddSceneCommand>("AddSceneCommand");
        GetNodeOrNull<Timer>("Timer")?.Start();
    }
    #endregion
}
