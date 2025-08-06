using Godot;
using teos.common.command;
using teos.common.system;
using teos.game.mob;

namespace teos.game.stage.character_manager;

/// <summary>
/// シークレットアイテム用のCharacterEnabler
/// </summary>
public partial class SecretCharacterEnabler : Area2D, ILife, ICharacterManagerEnabler
{
    [Export]
    public int Life { get; set; } = 1;

    [Export]
    public bool EnableOffScreen { get; set; } = false;

    protected VisibleOnScreenNotifier2D m_OnScreen;
    private CharacterManager _characterManager;
    private ICharacterManager _target;
    private bool _opened = false;

    public override void _Ready()
    {
        m_OnScreen = GetNodeOrNull<VisibleOnScreenNotifier2D>("OnScreen");
        _target = GetParentOrNull<ICharacterManager>();
    }

    protected virtual void PlaySe(string name)
    {
        if (m_OnScreen is null || m_OnScreen.IsOnScreen())
        {
            GetNode<SePlayer>("/root/SePlayer").Play(name);
        }
    }

    #region ILifeインタフェース
    public void AddLife(int value)
    {
        if (!_opened && (m_OnScreen is null || m_OnScreen.IsOnScreen()) && value < 0)
        {
            Life = Mathf.Clamp(Life + value, 0, int.MaxValue);

            if (Life > 0)
            {
                Damaged();
            }
            else
            {
                Dead();
            }
        }
    }

    public void Damaged()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("Damaged"), this, true);
    }

    public void Dead()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("Dead"), this, true);
        EnableCharacter();
        SetCollisionLayerValue(12, false);
    }
    #endregion

    #region ICharacterManagerEnablerインタフェース
    public void EnableCharacter()
    {
        if (_target is not null && _characterManager is not null)
        {
            _characterManager.EnableCharacterNode(_target, true);
            _opened = true;
        }
    }

    public ICharacterManager GetCharacter()
    {
        return _target;
    }

    public void SetCharacterManager(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public void ReparentCharacterEnabler(Node characterEnablerList)
    {
        Reparent(characterEnablerList);
    }
    #endregion
}
