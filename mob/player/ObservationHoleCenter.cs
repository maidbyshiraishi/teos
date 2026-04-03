using Godot;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.tilemap;

namespace maid_by_shiraishi.mob.player;

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
        AddToGroup(GameStageRoot.ProcessGroup);
    }

    public override void _Process(double delta) => Target?.ManageObservationHole(GlobalPosition);
}
