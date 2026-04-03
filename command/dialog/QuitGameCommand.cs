using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

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
