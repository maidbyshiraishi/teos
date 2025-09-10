using Godot;
using teos.mob.player;

namespace teos.command;

/// <summary>
/// プレーヤーの勝利アニメーションを再生する
/// </summary>
public partial class VictoryPlayerCommand : CommandRoot
{
    /// <summary>
    /// これでゲーム終了か
    /// </summary>
    [Export]
    public bool GameEnded { get; set; } = false;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        Player.GetPlayer(this).SetStageVictory(GameEnded);
    }
}
