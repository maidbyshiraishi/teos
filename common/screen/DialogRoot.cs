using Godot;
using Godot.Collections;
using teos.common.system;
using teos.common.trigger;

namespace teos.common.screen;

/// <summary>
/// 画面とダイアログの親
/// </summary>
public partial class DialogRoot : Control
{
    [Export]
    public bool MouseCaptured { get; set; } = false;

    protected Array<Variant> m_Argument = [];

    private Control _lastFocus = null;

    public override void _EnterTree()
    {
        _ = Connect(Node.SignalName.Ready, new(this, MethodName.InitializeNode));
        GetArgument();
    }

    public virtual void InitializeNode()
    {
    }

    public virtual void GetArgument()
    {
    }

    public void GetDialogArgument(string key)
    {
        m_Argument = GetNode<DialogArgument>("/root/DialogArgument").GetArgument(key);
    }

    public virtual void Close()
    {
        QueueFree();
    }

    public virtual void Active()
    {
        ChangeFocusMode(true);
        RestoreLastFocus();
        GetNodeOrNull<VisibleTrigger>("Focus")?.Show();
    }

    public virtual void Inactive()
    {
        SaveFocus();
        ChangeFocusMode(false);
        GetNodeOrNull<VisibleTrigger>("Focus")?.Hide();
    }

    /// <summary>
    /// フォーカスのあるControlを保存する
    /// </summary>
    public virtual void SaveFocus()
    {
        _lastFocus = GetFocus();
    }

    public virtual Control GetFocus(Control root = null)
    {
        Control control = root is null ? GetNodeOrNull<Control>("Control") : root.GetNodeOrNull<Control>("Control");

        if (control is null)
        {
            return null;
        }

        foreach (Node n in control.GetChildren())
        {
            if (n is Control c && c.HasFocus())
            {
                return c;
            }
        }

        return null;
    }

    public virtual void SetFocusFirst(Control root = null)
    {
        Control control = root is null ? GetNodeOrNull<Control>("Control") : root.GetNodeOrNull<Control>("Control");

        if (control is null)
        {
            return;
        }

        foreach (Node n in control.GetChildren())
        {
            if (n is Control c && c.Visible)
            {
                GrabFocus(c);
                return;
            }
        }
    }

    /// <summary>
    /// フォーカスを復元する
    /// 前回保存されたControlがなければ、
    /// 最初に発見したControlにフォーカスを設定する
    /// </summary>
    public virtual void RestoreLastFocus(Control root = null)
    {
        Control control = root is null ? GetNodeOrNull<Control>("Control") : root.GetNodeOrNull<Control>("Control");

        if (control is null)
        {
            return;
        }

        Array<Node> children = control.GetChildren();

        // 前回のControlが存在する場合            
        if (_lastFocus is not null && _lastFocus.Visible && children.Contains(_lastFocus))
        {
            GrabFocus(_lastFocus);
            return;
        }

        // 前回のControlが存在しない場合
        // デフォルトのControlを取得する
        string defaultNodeName = GetDefaultFocusNodeName();

        if (string.IsNullOrWhiteSpace(defaultNodeName))
        {
            SetFocusFirst(root);
            return;
        }

        foreach (Node n in children)
        {
            if (n is Control c && c.Visible && c.Name == defaultNodeName)
            {
                GrabFocus(c);
                return;
            }
        }

        SetFocusFirst(root);
    }

    public virtual void ClearLastFocus()
    {
        _lastFocus = null;
    }

    protected virtual string GetDefaultFocusNodeName()
    {
        return null;
    }

    protected void ChangeFocusMode(bool enabled, Control root = null)
    {
        Control control = root is null ? GetNodeOrNull<Control>("Control") : root.GetNodeOrNull<Control>("Control");

        if (control is null)
        {
            return;
        }

        control.Modulate = enabled ? Colors.White : Colors.DarkGray;

        foreach (Node n in control.GetChildren())
        {
            if (n is Control cnode)
            {
                cnode.FocusMode = enabled ? FocusModeEnum.All : FocusModeEnum.None;
            }
        }
    }

    public virtual void Undo()
    {
    }

    public virtual void UpdateDialogScreen()
    {
    }


    public static void GrabFocus(Control control)
    {
        control.SetBlockSignals(true);
        control.GrabFocus();
        control.SetBlockSignals(false);
    }

    public static void ConnectFocusSignal(Control control, Callable entered, Callable exited, Callable moudeEntered)
    {
        _ = control.Connect(Control.SignalName.FocusEntered, entered);
        _ = control.Connect(Control.SignalName.MouseEntered, moudeEntered);
        _ = control.Connect(Control.SignalName.FocusExited, exited);
        _ = control.Connect(Control.SignalName.MouseExited, exited);
    }
}
