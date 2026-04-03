using Godot;
using maid_by_shiraishi.mob.player;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// プレイヤーのライフを初期値に設定するコマンド
/// </summary>
public partial class ResetPlayerLifeCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        Player.GetPlayer(this).ResetLife();
    }
}
