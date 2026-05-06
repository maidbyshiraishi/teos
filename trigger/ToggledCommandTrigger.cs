using Godot;
using maid_by_shiraishi.command;
using static Godot.Control;

namespace maid_by_shiraishi.trigger;

/// <summary>
/// トグルコマンドトリガー
/// </summary>
public partial class ToggledCommandTrigger : CommandContainer
{
    private Control _control;

    public override void _Ready()
    {
        base._Ready();
        _control = GetParent<Control>();

        if (_control is BaseButton baseButton)
        {
            baseButton.Toggled += Toggled;
        }
    }

    public virtual void Toggled(bool toggledOn)
    {
        if (_control.FocusMode != FocusModeEnum.None)
        {
            ExecAllCommand(this, _control, toggledOn);
        }
    }
}
