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
            _ = Connect(Area2D.SignalName.AreaEntered, new(this, MethodName.ExecArea2D));
            _ = Connect(Area2D.SignalName.BodyEntered, new(this, MethodName.Exec));
        }

        if (AutoConnectExit)
        {
            _ = Connect(Area2D.SignalName.AreaExited, new(this, MethodName.ExecExitArea2D));
            _ = Connect(Area2D.SignalName.BodyExited, new(this, MethodName.ExecExit));
        }
    }

    public void Exec(Node2D node) => CommandRoot.ExecChildren(this, Target is null ? node : Target, true);

    public void ExecArea2D(Area2D node) => CommandRoot.ExecChildren(this, Target is null ? node : Target, true);

    public void ExecExit(Node2D node) => CommandRoot.ExecChildren(this, Target is null ? node : Target, false);

    public void ExecExitArea2D(Area2D node) => CommandRoot.ExecChildren(this, Target is null ? node : Target, false);
}
