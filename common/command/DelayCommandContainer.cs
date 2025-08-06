using Godot;

namespace teos.common.command;

/// <summary>
/// コマンド延滞実行コンテナ
/// </summary>
public partial class DelayCommandContainer : CommandRoot
{
    /// <summary>
    /// 延滞時間
    /// </summary>
    [Export]
    public double WaitTime { get; set; } = 0f;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        WaitExec(node, flag);
    }

    private async void WaitExec(Node node, bool flag)
    {
        if (WaitTime >= 0.05f)
        {
            _ = await ToSignal(GetTree().CreateTimer(WaitTime, false), Timer.SignalName.Timeout);
        }

        ExecAllCommand(this, node, flag);
    }
}
