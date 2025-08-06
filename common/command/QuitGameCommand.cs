using Godot;
using teos.common.system;

namespace teos.common.command;

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
