using Godot;
using teos.common.command;
using teos.common.screen;

namespace teos.common.gui;

/// <summary>
/// 押下とフォーカスの入出時にコマンドを実行するチェックボタン
/// </summary>
public partial class CommandCheckButton : CheckButton
{
    public override void _Ready()
    {
        _ = Connect(BaseButton.SignalName.Toggled, new(this, MethodName.ExecToggled));
        DialogRoot.ConnectFocusSignal(this, new(this, MethodName.ExecFocusEntered), new(this, MethodName.ExecFocusExited), new(this, MethodName.ExecMouseEntered));
    }

    public virtual void ExecToggled(bool toggledOn)
    {
        if (FocusMode != FocusModeEnum.None)
        {
            CommandRoot.ExecChildren(GetNodeOrNull("Toggled"), this, toggledOn);
        }
    }

    public virtual void ExecFocusEntered()
    {
        if (FocusMode != FocusModeEnum.None)
        {
            CommandRoot.ExecChildren(GetNodeOrNull("Focus"), this, true);
        }
    }

    public virtual void ExecMouseEntered()
    {
        GrabFocus();
    }

    public virtual void ExecFocusExited()
    {
        if (FocusMode != FocusModeEnum.None)
        {
            CommandRoot.ExecChildren(GetNodeOrNull("Focus"), this, false);
        }
    }
}
