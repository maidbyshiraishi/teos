using Godot;
using teos.common.command;
using teos.game.mob;

namespace teos.game.command;

/// <summary>
/// ドロップアイテムを有効化するコマンド
/// </summary>
public partial class DropItemCommand : CommandRoot
{
    /// <summary>
    /// アイテムをドロップする元ノード
    /// </summary>
    [Export]
    public Node Target { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (Target is IItemDropper dropper1)
        {
            dropper1.DropItem();
        }
        else if (node is IItemDropper dropper2)
        {
            dropper2.DropItem();
        }
    }
}
