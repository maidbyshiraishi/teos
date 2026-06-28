using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// 自動スクロールを停止するコマンド
/// </summary>
public partial class PauseAutoScrollCommand : CommandRoot
{
    /// <summary>
    /// 自動スクロールのポーズ状態
    /// </summary>
    [Export]
    public bool Paused { get; set; } = true;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot().PauseAutoScroll = Paused;
    }
}
