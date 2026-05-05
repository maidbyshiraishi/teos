using Godot;

namespace maid_by_shiraishi.command.timer;

/// <summary>
/// 一時停止されたタイマーを再開するコマンド
/// </summary>
public partial class ResumeTimerCommand : CommandRoot
{
    /// <summary>
    /// 一時停止を解除するタイマー
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
    }
}
