using Godot;
using teos.common.command;

namespace teos.common.trigger;

/// <summary>
/// 接触時にコマンドを実行するトリガー
/// </summary>
public partial class CollisionTrigger : Area2D
{
    [Export]
    public Node Target { get; set; }

    public void Exec(Node2D node)
    {
        CommandRoot.ExecChildren(this, Target is null ? node : Target, true);
    }

    public void ExecArea2D(Area2D node)
    {
        CommandRoot.ExecChildren(this, Target is null ? node : Target, true);
    }

    public void ExecExit(Node2D node)
    {
        CommandRoot.ExecChildren(this, Target is null ? node : Target, false);
    }

    public void ExecExitArea2D(Area2D node)
    {
        CommandRoot.ExecChildren(this, Target is null ? node : Target, false);
    }
}
