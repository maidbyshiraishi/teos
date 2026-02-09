using Godot;
using teos.stage;
using teos.tilemap;

namespace teos.mob.player;

/// <summary>
/// 前面ブロック透過中心点
/// </summary>
public partial class ObservationHoleCenter : Node2D, IGameNode
{
    [Export]
    public ObservationHole Target { get; set; }

    public override void _Ready()
    {
        AddToGroup(IGameNode.GameNodeGroup);
        AddToGroup(StageRoot.ProcessGroup);
    }

    public override void _Process(double delta) => Target?.ManageObservationHole(GlobalPosition);
}
