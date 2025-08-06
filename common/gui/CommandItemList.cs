using Godot;
using teos.common.command;
using teos.common.screen;

namespace teos.common.gui;

/// <summary>
/// フォーカスの入出時にコマンドを実行するアイテムリスト
/// </summary>
public partial class CommandItemList : ItemList
{
    public override void _Ready()
    {
        DialogRoot.ConnectFocusSignal(this, new(this, MethodName.ExecFocusEntered), new(this, MethodName.ExecFocusExited), new(this, MethodName.ExecMouseEntered));
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
