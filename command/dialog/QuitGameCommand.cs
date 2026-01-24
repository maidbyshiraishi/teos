using Godot;
using teos.system;

namespace teos.command.dialog;

/// <summary>
/// ゲームを終了するコマンド
/// </summary>
public partial class QuitGameCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").QuitGame();
    }
}
