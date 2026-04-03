using Godot;
using maid_by_shiraishi.command;

namespace maid_by_shiraishi.trigger;

/// <summary>
/// 表示でコマンドを実行するトリガー
/// </summary>
public partial class VisibleTrigger : VisibleOnScreenNotifier2D
{
    [Export]
    public Node Target { get; set; }

    public override void _Ready()
    {
        ScreenEntered += Entered;
        ScreenExited += Exited;
    }

    public void Entered() => CommandRoot.ExecChildren(this, Target, true);

    public void Exited() => CommandRoot.ExecChildren(this, Target, false);
}
