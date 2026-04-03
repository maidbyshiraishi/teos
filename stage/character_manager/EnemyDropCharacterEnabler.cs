using Godot;
using maid_by_shiraishi.item;
using maid_by_shiraishi.mob;

namespace maid_by_shiraishi.stage.character_manager;

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
