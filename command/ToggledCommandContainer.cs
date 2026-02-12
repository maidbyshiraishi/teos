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
