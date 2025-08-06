using Godot;

namespace teos.game.stage.character_manager;

/// <summary>
/// 画面中央のマーカーと接触することで起動するCharacterEnabler
/// </summary>
public partial class CenterOfCameraCharacterEnabler : Area2D, ICharacterManagerEnabler
{
    [Export]
    public int Count { get; set; } = 1;

    private CharacterManager _characterManager;
    private ICharacterManager _target;
    private int _count = 0;

    public override void _Ready()
    {
        _target = GetParentOrNull<ICharacterManager>();
        _ = Connect(Area2D.SignalName.AreaEntered, new(this, MethodName.EnterArea2D));
    }

    public void EnterArea2D(Area2D area)
    {
        _count++;

        if (Count == _count)
        {
            EnableCharacter();
        }
    }

    #region ICharacterManagerEnablerインタフェース
    public void EnableCharacter()
    {
        _characterManager?.EnableCharacterNode(_target, true);
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
