using Godot;
using teos.common.decoration;

namespace teos.game.command;

/// <summary>
/// フローティングメッセージを表示するコマンド
/// </summary>
public partial class ShowFloatingMessageCommand : ShowDecorationCommand
{
    /// <summary>
    /// 表示するメッセージ
    /// </summary>
    [Export]
    public string Message { get; set; }

    /// <summary>
    /// 表示色
    /// </summary>
    [Export]
    public Color Color { get; set; } = Colors.White;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || string.IsNullOrWhiteSpace(Path) || string.IsNullOrWhiteSpace(Message))
        {
            return;
        }

        if (node is Node2D node2d && Lib.GetPackedScene(Path) is PackedScene pack && pack.Instantiate() is FloatingMessage decoration)
        {
            decoration.Text = Message;
            decoration.Color = Color;
            AddNode(node2d, decoration);
        }
    }
}
