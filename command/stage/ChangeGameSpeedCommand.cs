using Godot;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// ゲーム速度を変更するコマンド
/// </summary>
public partial class ChangeGameSpeedCommand : CommandRoot
{
    /// <summary>
    /// 動作倍率
    /// </summary>
    [Export]
    public double Scale { get; set; } = 1.0f;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        Engine.TimeScale = Scale;
    }
}
