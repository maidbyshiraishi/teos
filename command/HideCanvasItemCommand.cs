using Godot;

namespace teos.command;

/// <summary>
/// CanvasItemを隠すコマンド
/// </summary>
public partial class HideCanvasItemCommand : CommandRoot
{
    /// <summary>
    /// 隠すCanvasItem
    /// </summary>
    [Export]
    public CanvasItem Target { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        Target?.Hide();
    }
}
