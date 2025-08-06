namespace teos.game.stage.character_manager;

/// <summary>
/// キャラクターマネージャーによる管理対象のインタフェース
/// CharacterEnablerから有効化・無効化が制御される。
/// </summary>
public interface ICharacterManager
{
    #region ICharacterManagerインタフェース
    /// <summary>
    /// CharacterManagerをセットする
    /// </summary>
    /// <param name="characterManager">CharacterManager</param>
    void SetCharacterManager(CharacterManager characterManager) { }

    /// <summary>
    /// キャラクターを有効化または無効化する
    /// </summary>
    /// <param name="active">有効または無効</param>
    void ActiveCharacter(bool active) { }

    /// <summary>
    /// キャラクターを初期化する
    /// ステートマシン内で実行され、CharacterManagerからは直接実行されない。
    /// </summary>
    void InitializeCharacter() { }

    /// <summary>
    /// キャラクターを終了する
    /// ステートマシン内で実行され、CharacterManagerからは直接実行されない。
    /// </summary>
    void TerminateCharacter() { }
    #endregion
}
