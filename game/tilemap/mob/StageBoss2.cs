using Godot;
using Godot.Collections;
using teos.game.stage.character_manager;

namespace teos.game.tilemap.mob;

/// <summary>
/// ステージ2ボス
/// </summary>
public partial class StageBoss2 : TileMapMob
{
    private int _state = 0;
    private float _angle;
    private Array<ulong> _piles = [];
    private int _pileIndex = 0;

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

    public void ActivePile()
    {
        if (_pileIndex < _piles.Count && IsInstanceIdValid(_piles[_pileIndex]) && InstanceFromId(_piles[_pileIndex]) is ICharacterManager character)
        {
            _pileIndex++;
            character.ActiveCharacter(true);
            return;
        }

        GetNode<Timer>("ActivePileTimer").Stop();
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
            if (n is EnemyDropCharacterEnabler || n is not ManualCharacterEnabler characterEnabler)
            {
                continue;
            }

            characterEnabler.EnableCharacter();
            ICharacterManager character = characterEnabler.GetCharacter();
            character.ActiveCharacter(false);

            if (character is Node node && node.Name == "Pile")
            {
                _piles.Add(node.GetInstanceId());
            }
        }

        GetNodeOrNull<Timer>("StartRotateTimer")?.Start();
    }
    #endregion
}
