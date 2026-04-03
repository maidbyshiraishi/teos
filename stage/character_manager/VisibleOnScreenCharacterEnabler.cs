using Godot;

namespace maid_by_shiraishi.stage.character_manager;

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

        if (EnableOffScreen)
        {
            ScreenExited += EnableCharacter;
        }
        else
        {
            ScreenEntered += EnableCharacter;
        }
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

    public ICharacterManager GetCharacter() => _target;

    public void SetCharacterManager(CharacterManager characterManager) => _characterManager = characterManager;

    public void ReparentCharacterEnabler(Node characterEnablerList) => Reparent(ReparentNode is not null ? ReparentNode : characterEnablerList);
    #endregion
}
