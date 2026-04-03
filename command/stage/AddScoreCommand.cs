using Godot;
using maid_by_shiraishi.mob.player;

namespace maid_by_shiraishi.command.stage;

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
