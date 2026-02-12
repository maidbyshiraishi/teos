using Godot;
using teos.command;

namespace teos.trigger;

/// <summary>
/// 接触時にコマンドを実行するトリガー
/// </summary>
public partial class CollisionTrigger : Area2D
{
    [Export]
    public bool AutoConnectEnter { get; set; } = true;

    [Export]
    public bool AutoConnectExit { get; set; } = true;

    [Export]
    public Node Target { get; set; }

    public override void _Ready()
    {
        if (AutoConnectEnter)
        {
            AreaEntered += ExecArea2D;
            BodyEntered += Exec;
        }

        if (AutoConnectExit)
        {
            AreaExited += ExecExitArea2D;
            BodyExited += ExecExit;
        }
    }

    public void Exec(Node2D node) => CommandRoot.ExecChildren(this, Target is null ? node : Target, true);

    public void ExecArea2D(Area2D node) => CommandRoot.ExecChildren(this, Target is null ? node : Target, true);

    public void ExecExit(Node2D node) => CommandRoot.ExecChildren(this, Target is null ? node : Target, false);

    public void ExecExitArea2D(Area2D node) => CommandRoot.ExecChildren(this, Target is null ? node : Target, false);
}
