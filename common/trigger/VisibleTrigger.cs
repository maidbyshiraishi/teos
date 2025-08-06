using Godot;
using teos.common.command;

namespace teos.common.trigger;

/// <summary>
/// 表示でコマンドを実行するトリガー
/// </summary>
public partial class VisibleTrigger : VisibleOnScreenNotifier2D
{
    [Export]
    public Node Target { get; set; }

    public override void _Ready()
    {
        _ = Connect(VisibleOnScreenNotifier2D.SignalName.ScreenEntered, new(this, MethodName.Entered));
        _ = Connect(VisibleOnScreenNotifier2D.SignalName.ScreenExited, new(this, MethodName.Exited));
    }

    public void Entered()
    {
        CommandRoot.ExecChildren(this, Target, true);
    }

    public void Exited()
    {
        CommandRoot.ExecChildren(this, Target, false);
    }
}
