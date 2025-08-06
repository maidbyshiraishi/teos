using Godot;
using teos.common.command;
using teos.common.stage;
using teos.game.stage;
using teos.game.system;

namespace teos.game.command;

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
        GetNode<GameDataManager>("/root/GameDataManager").GetStageData().StageNo = DestStageNo;
        GetNode<GameDataManager>("/root/GameDataManager").Backup();
        GetNode<GameDialogLayer>("/root/DialogLayer").OpenGame(StartGameType.TravelStage, GameDataManager.NullSlotNo, Fadeout, Fadein);
    }
}
