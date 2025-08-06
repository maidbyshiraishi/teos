using Godot;
using teos.common.command;

namespace teos.common.trigger;

/// <summary>
/// タイマートリガー
/// </summary>
public partial class TimerTrigger : Node
{
    [Export]
    public Node Target { get; set; }

    [Export]
    public bool SwapExecFlag { get; set; } = false;

    [Export]
    public bool ExecFlag { get; set; } = true;

    private Timer _timer;

    public override void _Ready()
    {
        _timer = GetParentOrNull<Timer>();
        _ = _timer?.Connect(Timer.SignalName.Timeout, new(this, MethodName.Exec));
    }

    public virtual void Exec()
    {
        CommandRoot.ExecChildren(this, Target is null ? _timer : Target, ExecFlag);
        ExecFlag = SwapExecFlag ? !ExecFlag : ExecFlag;
    }

    /// <summary>
    /// タイマーを再スタートする
    /// </summary>
    public void ResetTimer()
    {
        if (_timer is not null && Mathf.Abs(_timer.WaitTime) >= 0.05f)
        {
            _timer.Paused = false;
            _ = _timer.CallDeferred(Timer.MethodName.Start);
        }
    }

    /// <summary>
    /// タイマーを一時停止する
    /// </summary>
    /// <param name="paused">停止するか</param>
    public void PauseTimer(bool paused)
    {
        if (_timer is not null)
        {
            _timer.Paused = paused;
        }
    }
}
