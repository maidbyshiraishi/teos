using Godot;
using teos.common.command;
using teos.game.stage.hud;
using teos.game.system;

namespace teos.game.command;

/// <summary>
/// HUDメッセージを表示するコマンド
///メッセージは3行表示可能
/// </summary>
public partial class ShowHudMessageCommand : CommandRoot
{
    /// <summary>
    /// HUDメッセージ1行目
    /// </summary>
    [Export]
    public string Text1 { get; set; }

    /// <summary>
    /// HUDメッセージ2行目
    /// </summary>
    [Export]
    public string Text2 { get; set; }

    /// <summary>
    /// HUDメッセージ3行目
    /// </summary>
    [Export]
    public string Text3 { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        Hud hud = GetNode<GameDialogLayer>("/root/DialogLayer").GetCurrentGameRoot().GetNode<Hud>("Hud");
        hud.SetMessage(Text1, Text2, Text3);
        hud.ShowMessage();
    }
}
