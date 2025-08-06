using Godot;

namespace teos.game.stage.character_manager;

/// <summary>
/// 手動制御用のCharacterEnabler
/// </summary>
public partial class ManualCharacterEnabler : Node, ICharacterManagerEnabler
{
    [Export]
    public Node Parent { get; set; }

    protected ICharacterManager m_Target;

    private CharacterManager _characterManager;

    public override void _Ready()
    {
        m_Target = GetParentOrNull<ICharacterManager>();
    }

    #region ICharacterManagerEnablerインタフェース
    public void EnableCharacter()
    {
        _characterManager?.EnableCharacterNode(m_Target, true);
    }

    public ICharacterManager GetCharacter()
    {
        return m_Target;
    }

    public void SetCharacterManager(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public void ReparentCharacterEnabler(Node characterEnablerList)
    {
        Reparent(Parent);
    }
    #endregion
}
