using Godot;

namespace teos.game.tilemap.mob;

public partial class BossController : Area2D
{
    private TileMapMob _boss;

    public override void _Ready()
    {
        if (GetParent() is TileMapMob boss)
        {
            _boss = boss;
            _ = Connect(Area2D.SignalName.AreaEntered, new(this, MethodName.AdvanceBoss));
        }
    }

    public void AdvanceBoss(Area2D area)
    {
        if (_boss is not null && area is AdvanceBossArea abcommand)
        {
            _boss.AdvanceBossState(abcommand.State, abcommand.Value);
        }
    }
}
