using Godot;

namespace teos.common.command;

/// <summary>
/// タイマーを開始するコマンド
/// 一時停止も解除される
/// </summary>
public partial class StartTimerCommand : CommandRoot
{
    /// <summary>
    /// 開始するタイマー
    /// </summary>
    [Export]
    public Timer Target { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || Target is null)
        {
            return;
        }

        Target.Paused = false;
        Target.Start();
    }
}
