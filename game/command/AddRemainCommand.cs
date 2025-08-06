using Godot;
using teos.common.command;
using teos.game.mob.player;

namespace teos.game.command;

/// <summary>
/// 残機を増やすコマンド
/// </summary>
public partial class AddRemainCommand : CommandRoot
{
    /// <summary>
    /// 残機の増減
    /// </summary>
    [Export]
    public int Value { get; set; } = 1;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        Player.GetPlayer(this).AddRemain(Value);
    }
}
