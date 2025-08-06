using Godot;
using teos.game.item;
using teos.game.mob;

namespace teos.game.stage.character_manager;

/// <summary>
/// 敵破壊時に起動するCharacterEnabler
/// </summary>
public partial class EnemyDropCharacterEnabler : ManualCharacterEnabler
{
    public override void _Ready()
    {
        if (GetParent() is Node parent && parent.GetParent() is Node node)
        {
            if (parent is ItemPackRoot carrier)
            {
                m_Target = carrier;
            }

            Parent = node;

            if (node is IItemDropper itemDropper)
            {
                itemDropper.AddItemDropper(this);
            }
        }
    }
}
