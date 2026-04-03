using Godot;

namespace maid_by_shiraishi.stage.character_manager;

/// <summary>
/// キャラクター有効化判定処理のインタフェース
/// </summary>
public interface ICharacterManagerEnabler
{
    #region ICharacterManagerEnablerインタフェース
    void EnableCharacter();

    ICharacterManager GetCharacter();

    void SetCharacterManager(CharacterManager characterManager);

    void ReparentCharacterEnabler(Node characterEnablerList);
    #endregion
}
