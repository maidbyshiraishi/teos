using Godot;

namespace teos.game.mob.tentacle;

/// <summary>
/// 触手の首
/// </summary>
public partial class TentacleNeck : Node2D
{
    [Export]
    public Node2D MobPrevious { get; set; }

    [Export]
    public Node2D MobNext { get; set; }

    private bool _disable = false;

    public override void _Process(double delta)
    {
        if (_disable || MobPrevious is null || MobNext is null)
        {
            return;
        }

        GlobalPosition = (MobPrevious.GlobalPosition + MobNext.GlobalPosition) / 2f;
    }

    public bool HasISweepNode()
    {
        foreach (Node n in GetChildren())
        {
            if (n is ISweep)
            {
                return true;
            }
        }

        return false;
    }

    public void SweepAll()
    {
        _disable = true;

        foreach (Node n in GetChildren())
        {
            if (IsInstanceValid(n) && n is ISweep sweep)
            {
                sweep.Sweep();
            }
        }
    }
}

