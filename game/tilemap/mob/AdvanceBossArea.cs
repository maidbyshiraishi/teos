using Godot;

namespace teos.game.tilemap.mob;

public partial class AdvanceBossArea : Area2D
{
    [Export]
    public int State { get; set; }

    [Export]
    public int Value { get; set; }
}
