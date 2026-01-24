using Godot;
using teos.mob.player;

namespace teos.command.stage;

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
