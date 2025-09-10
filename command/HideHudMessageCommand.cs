using Godot;
using teos.stage.hud;
using teos.system;

namespace teos.command;

/// <summary>
/// HUDメッセージを非表示するコマンド
/// </summary>
public partial class HideHudMessageCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<GameDialogLayer>("/root/DialogLayer").GetCurrentGameRoot().GetNode<Hud>("Hud").HideMessage();
    }
}
