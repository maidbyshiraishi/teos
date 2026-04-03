using Godot;
using maid_by_shiraishi.stage.hud;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.hud;

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

        GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot().GetNode<Hud>("Hud").HideMessage();
    }
}
