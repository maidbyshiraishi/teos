using Godot;
using teos.common.command;
using teos.game.mob.player;

namespace teos.game.command;

/// <summary>
/// 得点を操作するコマンド
/// </summary>
public partial class AddScoreCommand : CommandRoot
{
    /// <summary>
    /// 得点
    /// </summary>
    [Export]
    public int Score { get; set; } = 3000;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || Score == 0)
        {
            return;
        }

        Player.GetPlayer(this).AddScore(Score);
    }
}
