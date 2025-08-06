using Godot;

namespace teos.game.mob.tentacle;

/// <summary>
/// 触手の頭
/// </summary>
public partial class TentacleHead : Node2D
{
    [Export]
    public Node2D MobPrevious { get; set; }

    [Export]
    public TentacleRoot TentacleRoot { get; set; }

    [Export]
    public float MaxNeckLength { get; set; } = 100f;

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

    public void TrimLength()
    {
        Vector2 position = Position;
        position = position.LimitLength(MaxNeckLength);
        Position = position;
    }
}

