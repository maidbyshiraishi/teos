using Godot;
using teos.command;

namespace teos.trigger;

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
        if (GetParent() is Timer timer)
        {
            _timer = timer;
            _timer.Timeout += Exec;
        }
    }

    public virtual void Exec()
    {
        if (Target is not null)
        {
            CommandRoot.ExecChildren(this, Target, ExecFlag);
        }
        else if (_timer is not null)
        {
            CommandRoot.ExecChildren(this, _timer, ExecFlag);
        }

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
