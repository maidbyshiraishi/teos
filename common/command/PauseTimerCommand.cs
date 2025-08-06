using Godot;

namespace teos.common.command;

/// <summary>
/// タイマーを一時停止するコマンド
/// </summary>
public partial class PauseTimerCommand : CommandRoot
{
    /// <summary>
    /// 一時停止するタイマー
    /// </summary>
    [Export]
    public Timer Target { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || Target is null)
        {
            return;
        }

        Target.Paused = true;
    }
}
