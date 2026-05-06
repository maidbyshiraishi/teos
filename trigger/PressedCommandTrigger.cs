using Godot;
using maid_by_shiraishi.command;
using static Godot.Control;

namespace maid_by_shiraishi.trigger;

/// <summary>
/// 押下コマンドトリガー
/// </summary>
public partial class PressedCommandTrigger : CommandContainer
{
    private Node _node;

    public override void _Ready()
    {
        base._Ready();
        _node = GetParent();

        if (_node is null)
        {
            return;
        }

        // Godotデフォ由来は小文字スタート
        if (_node.HasSignal("pressed"))
        {
            _ = _node.Connect("pressed", new(this, MethodName.Pressed));
            return;
        }

        // C#由来は大文字スタート
        if (_node.HasSignal("Pressed"))
        {
            _ = _node.Connect("Pressed", new(this, MethodName.Pressed));
            return;
        }
    }

    public virtual void Pressed()
    {
        switch (_node)
        {
            case null:
                return;

            case Control control:

                // ノードがControlの場合はフォーカスモードを確認する
                if (control.FocusMode == FocusModeEnum.None)
                {
                    return;
                }

                break;

            case CanvasItem canvasItem:

                // ノードがCanvasItemの場合は画面に表示されているかを確認する
                if (!canvasItem.IsVisibleInTree())
                {
                    return;
                }

                break;
        }

        ExecAllCommand(this, _node, true);
    }
}
