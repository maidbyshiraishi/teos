using Godot;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// ステージを変更するコマンド
/// </summary>
public partial class TravelStageCommand : CommandRoot
{
    /// <summary>
    /// フェードアウトエフェクト
    /// </summary>
    [Export]
    public string Fadeout { get; set; } = "fadeout_1";

    /// <summary>
    /// フェードインエフェクト
    /// </summary>
    [Export]
    public string Fadein { get; set; } = "fadein_1";

    /// <summary>
    /// 移動先ステージ番号
    /// </summary>
    [Export]
    public int DestStageNo { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetTree().CallGroup(IStateful.StatefulGroup, "StateSave");
        SystemHub.GetGameDataManager().GetStageData().StageNo = DestStageNo;
        SystemHub.GetGameDataManager().Backup();
        SystemHub.GetDialogLayer().OpenGame(StartGameType.TravelStage, GameDataManager.NullSlotNo, Fadeout, Fadein);
    }
}
