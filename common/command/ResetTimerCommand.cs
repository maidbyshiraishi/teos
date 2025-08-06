using Godot;

namespace teos.common.command;

/// <summary>
/// タイマーをリセットして再開するコマンド
/// </summary>
public partial class ResetTimerCommand : CommandRoot
{
    /// <summary>
    /// リセットするタイマー
    /// </summary>
    [Export]
    public Timer Target { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || Target is null || Mathf.Abs(Target.WaitTime) < 0.05f)
        {
            return;
        }

        Target.Paused = false;
        _ = Target.CallDeferred(Timer.MethodName.Start);
    }
}
