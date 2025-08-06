using Godot;
using teos.common.command;

namespace teos.common.trigger;

/// <summary>
/// トリガーArea2D版
/// 検出される側であり、接触しても、こちら側からは何も行わず、接触側のEventFinderでコマンドの呼び出しなどを行う。
/// </summary>
public partial class TriggerArea2D : Area2D
{
    /// <summary>
    /// 接触が継続して行われるか
    /// </summary>
    [Export]
    public bool Continuous { get; set; } = false;

    /// <summary>
    /// 継続接触の間隔
    /// </summary>
    [Export]
    public double WaitTime { get; set; }

    /// <summary>
    /// 有効・無効
    /// </summary>
    [Export]
    public bool Disable { get; set; } = false;

    /// <summary>
    /// 一度のみトリガーを実行する
    /// </summary>
    [Export]
    public bool OneTime { get; set; } = true;

    [Export]
    public Node Target { get; set; }

    protected bool m_Opened = false;

    public virtual void Exec(Node2D node)
    {
        if (Disable || (OneTime && m_Opened))
        {
            return;
        }

        if (OneTime)
        {
            m_Opened = true;
            SetCollisionMaskValue(10, false);
        }

        CommandRoot.ExecChildren(this, Target is null ? node : Target, true);
    }

    public virtual void SetOpened(bool opened)
    {
        m_Opened = opened;
    }
}
