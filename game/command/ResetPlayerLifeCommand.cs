using Godot;
using teos.common.command;
using teos.game.mob.player;

namespace teos.game.command;

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
