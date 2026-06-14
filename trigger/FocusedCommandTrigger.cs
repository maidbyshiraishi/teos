using Godot;
using maid_by_shiraishi.command;
using static Godot.Control;

namespace maid_by_shiraishi.trigger;

/// <summary>
/// フォーカスコマンドトリガー
/// </summary>
public partial class FocusedCommandTrigger : CommandContainer
{
    private Control _control;

    public override void _Ready()
    {
        base._Ready();

        if (GetParent() is Control control)
        {
            _control = control;

            if (_control.HasSignal(Control.SignalName.FocusEntered))
            {
                _control.FocusEntered += ExecFocusEntered;
            }

            if (_control.HasSignal(Control.SignalName.FocusExited))
            {
                _control.FocusExited += ExecFocusExited;
            }

            if (_control.HasSignal(Control.SignalName.MouseEntered))
            {
                _control.MouseEntered += ExecMouseEntered;
            }
        }
    }

    public virtual void ExecFocusEntered()
    {
        if (_control.FocusMode != FocusModeEnum.None)
        {
            ExecAllCommand(this, _control, true);
        }
    }

    public virtual void ExecFocusExited()
    {
        if (_control.FocusMode != FocusModeEnum.None)
        {
            ExecAllCommand(this, _control, false);
        }
    }

    public virtual void ExecMouseEntered()
    {
        if (_control.FocusMode != FocusModeEnum.None)
        {
            _control.GrabFocus();
        }
    }
}
