using Godot;
using teos.stage.character_manager;

namespace teos.command;

/// <summary>
/// ICharacterManagerのTerminateCharacter()を実行する
/// </summary>
public partial class TerminateCharacterCommand : CommandRoot
{
    /// <summary>
    /// フラグ名
    /// </summary>
    [Export]
    public Node2D Target { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || Target is not ICharacterManager character)
        {
            return;
        }

        character.TerminateCharacter();
    }
}
