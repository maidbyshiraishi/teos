using Godot;

namespace maid_by_shiraishi.command;

/// <summary>
/// ボタンを押下するコマンド
/// </summary>
public partial class ButtonPressedCommand : CommandRoot
{
    /// <summary>
    /// 押下するボタン
    /// </summary>
    [Export]
    public BaseButton Target { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        _ = (Target?.EmitSignal(BaseButton.SignalName.Pressed));
    }
}
