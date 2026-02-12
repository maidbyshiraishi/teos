using Godot;
using static Godot.Control;

namespace teos.command;

/// <summary>
/// 押下コマンドコンテナ
/// </summary>
public partial class PressedCommandContainer : CommandContainer
{
    private Control _control;

    public override void _Ready()
    {
        base._Ready();
        _control = GetParent<Control>();

        if (_control is BaseButton baseButton)
        {
            baseButton.Pressed += Pressed;
        }
    }

    public virtual void Pressed()
    {
        if (_control.FocusMode != FocusModeEnum.None)
        {
            ExecAllCommand(this, _control, true);
        }
    }
}
