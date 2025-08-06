using Godot;
using teos.common.command;
using teos.common.screen;

namespace teos.common.gui;

/// <summary>
/// 押下とフォーカスの入出時にコマンドを実行するボタン
/// </summary>
public partial class CommandButton : Button
{
    // todo: ボタンなどの各種シグナルを自動的に接続したい。

    public override void _Ready()
    {
        _ = Connect(BaseButton.SignalName.Pressed, new(this, MethodName.Exec));
        DialogRoot.ConnectFocusSignal(this, new(this, MethodName.ExecFocusEntered), new(this, MethodName.ExecFocusExited), new(this, MethodName.ExecMouseEntered));
    }

    public virtual void Exec()
    {
        if (FocusMode != FocusModeEnum.None)
        {
            CommandRoot.ExecChildren(GetNodeOrNull("Exec"), this, true);
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
        if (FocusMode != FocusModeEnum.None)
        {
            GrabFocus();
        }
    }

    public virtual void ExecFocusExited()
    {
        if (FocusMode != FocusModeEnum.None)
        {
            CommandRoot.ExecChildren(GetNodeOrNull("Focus"), this, false);
        }
    }
}
