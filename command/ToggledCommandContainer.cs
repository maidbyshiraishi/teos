using Godot;
using static Godot.Control;

namespace teos.command;

/// <summary>
/// コマンドコンテナ
/// </summary>
public partial class ToggledCommandContainer : CommandContainer
{
    private Control _control;

    public override void _Ready()
    {
        base._Ready();
        _control = GetParent<Control>();

        if (_control is not null && _control.HasSignal(BaseButton.SignalName.Toggled))
        {
            _ = _control.Connect(BaseButton.SignalName.Toggled, new(this, MethodName.Toggled));
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
