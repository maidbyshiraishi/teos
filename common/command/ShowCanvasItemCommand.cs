using Godot;
using teos.common.screen;

namespace teos.common.command;

/// <summary>
/// CanvasItemを表示するコマンド
/// </summary>
public partial class ShowCanvasItemCommand : CommandRoot
{
    /// <summary>
    /// 対表示するCanvasItem
    /// </summary>
    [Export]
    public CanvasItem Target { get; set; }

    /// <summary>
    /// フォーカスを設定するか
    /// </summary>
    [Export]
    public bool GrubFocus { get; set; } = false;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || Target is null)
        {
            return;
        }

        Target.Show();

        if (GrubFocus && Target is Control control)
        {
            DialogRoot.GrabFocus(control);
        }
    }
}
