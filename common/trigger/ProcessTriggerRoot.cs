using Godot;
using teos.common.command;
using teos.common.stage;

namespace teos.common.trigger;

/// <summary>
/// Processトリガーの親
/// </summary>
public partial class ProcessTriggerRoot : Node
{
    [Export]
    public Node Target { get; set; }

    public override void _Ready()
    {
        AddToGroup(StageRoot.ProcessGroup);
    }

    public override void _Process(double delta)
    {
        Exec();
    }

    public void Exec()
    {
        CommandRoot.ExecChildren(this, Target, true);
    }
}
