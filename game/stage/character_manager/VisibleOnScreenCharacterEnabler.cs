using Godot;

namespace teos.game.stage.character_manager;

/// <summary>
/// 画面内への進入と退出で起動するCharacterEnabler
/// </summary>
public partial class VisibleOnScreenCharacterEnabler : VisibleOnScreenNotifier2D, ICharacterManagerEnabler
{
    [Export]
    public bool EnableOffScreen { get; set; } = false;

    [Export]
    public Node ReparentNode { get; set; }

    private CharacterManager _characterManager;
    private ICharacterManager _target;

    public override void _Ready()
    {
        _target = GetParentOrNull<ICharacterManager>();
        _ = Connect(EnableOffScreen ? VisibleOnScreenNotifier2D.SignalName.ScreenExited : VisibleOnScreenNotifier2D.SignalName.ScreenEntered, new(this, MethodName.EnableCharacter));
    }

    #region ICharacterManagerEnablerインタフェース
    public void EnableCharacter()
    {
        if (_target is not null && _characterManager is not null)
        {
            _characterManager.EnableCharacterNode(_target, true);
            Visible = false;
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
        Reparent(ReparentNode is not null ? ReparentNode : characterEnablerList);
    }
    #endregion
}
