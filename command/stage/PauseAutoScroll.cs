using Godot;
using teos.system;

namespace teos.command.stage;

/// <summary>
/// 自動スクロールを停止するコマンド
/// </summary>
public partial class PauseAutoScroll : CommandRoot
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
