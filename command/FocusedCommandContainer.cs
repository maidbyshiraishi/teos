using Godot;
using static Godot.Control;

namespace teos.command;

/// <summary>
/// フォーカスコマンドコンテナ
/// </summary>
public partial class FocusedCommandContainer : CommandContainer
{
    private Control _control;

    public override void _Ready()
    {
        base._Ready();
        _control = GetParent<Control>();

        if (_control.HasSignal(Control.SignalName.FocusEntered))
        {
            _ = _control.Connect(Control.SignalName.FocusEntered, new(this, MethodName.ExecFocusEntered));
        }

        if (_control.HasSignal(Control.SignalName.FocusExited))
        {
            _ = _control.Connect(Control.SignalName.FocusExited, new(this, MethodName.ExecFocusExited));
        }

        if (_control.HasSignal(Control.SignalName.MouseEntered))
        {
            _ = _control.Connect(Control.SignalName.MouseEntered, new(this, MethodName.ExecMouseEntered));
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
