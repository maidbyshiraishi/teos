using Godot;
using teos.command;
using teos.stage;

namespace teos.trigger;

/// <summary>
/// Processトリガーの親
/// </summary>
public partial class ProcessTriggerRoot : Node
{
    [Export]
    public Node Target { get; set; }

    public override void _Ready() => AddToGroup(GameStageRoot.ProcessGroup);

    public override void _Process(double delta) => Exec();

    public void Exec() => CommandRoot.ExecChildren(this, Target, true);
}
