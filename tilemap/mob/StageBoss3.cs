using Godot;
using Godot.Collections;
using teos.command;

namespace teos.tilemap.mob;

/// <summary>
/// ステージ3ボス
/// </summary>
public partial class StageBoss3 : TileMapMob
{
    private int _state = 0;
    private float _angle;
    private int _pileIndex = 0;
    private Array<Marker2D> _marker = [];
    private int _markerCount = 0;
    private int _index = 0;
    private AddSceneCommand _addSceneCommand;

    public override void _Ready()
    {
        base._Ready();

        // Godotエディタからシグナルを接続すると
        // リリースビルドのエクスポート時、接続が失われることがある。
        if (GetNodeOrNull("Timer") is Timer timer)
        {
            timer.Timeout += MakeTentacle;
        }
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

    public void StartRotate() => AdvanceBossState(1, 30);

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
        if (_markerCount == 0)
        {
            return;
        }

        if (_marker[_index].GetChildCount() < 4)
        {
            _addSceneCommand.ParentNode = _marker[_index];
            _addSceneCommand.AddScene();
        }

        _index = (_index + 1) % _markerCount;
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
                _marker.Add(marker);
            }
        }

        _markerCount = _marker.Count;
        _addSceneCommand = GetNodeOrNull<AddSceneCommand>("AddSceneCommand");
        GetNodeOrNull<Timer>("Timer")?.Start();
    }
    #endregion
}
