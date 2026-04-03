using Godot;
using maid_by_shiraishi.command.hud;
using maid_by_shiraishi.data;
using maid_by_shiraishi.mob.player;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// ステージクリアボーナスを操作するコマンド
/// </summary>
public partial class AddScoreBonusCommand : ShowHudMessageCommand
{
    /// <summary>
    /// 得点倍率
    /// </summary>
    [Export]
    public int ScoreRate { get; set; } = 10000;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || ScoreRate == 0)
        {
            return;
        }

        PlayerData playerData = GetNode<GameDataManager>("/root/GameDataManager").GetPlayerData();
        int remain = playerData.Remain;
        Player.GetPlayer(this).AddScore(remain * ScoreRate);
        Text1 = "ステージクリアボーナス";
        Text2 = $"残機 {remain} × {ScoreRate}";
        Text3 = $"{remain * ScoreRate}";
        base.ExecCommand(node, flag);
    }
}
