using Godot;
using teos.common.command;
using teos.common.stage;
using teos.common.system;
using teos.game.stage;
using teos.game.system;

namespace teos.game.mob;

/// <summary>
/// キャラクターの親
/// </summary>
public partial class Mob : Area2D, IGameNode, ILife
{
    [Signal]
    public delegate void SceneAddedEventHandler(Node node, string parentNodePath);

    [Export]
    public bool RemoveScreenExited { get; set; } = true;

    [ExportGroup("Life")]

    [Export]
    public int InitialLife { get; set; } = 2;

    [Export]
    public int MaxLife { get; set; } = 10;

    [ExportGroup("Speed")]

    /// <summary>
    /// 初期値、最大速度に到達する距離
    /// </summary>
    [Export]
    public float InitialApproach { get; set; } = 16f;

    /// <summary>
    /// 初期値、最大速度
    /// </summary>
    [Export]
    public float InitialMaxSpeed { get; set; } = 100f;

    /// <summary>
    /// 初期値、減速に要する距離
    /// </summary>
    [Export]
    public float InitialReductionApproach { get; set; } = 16f;

    public int Life { get; set; }

    protected float m_Approach;
    protected float m_MaxSpeed;
    protected float m_ReductionApproach;
    protected float m_Acceleration;
    protected float m_ReductionAcceleration;
    protected VisibleOnScreenNotifier2D m_OnScreen;
    protected bool m_AMomentWaited = false;

    private Vector2 _direction = Vector2.Right;

    public override void _Ready()
    {
        ActivateOnScreen();
        AddToGroup(IGameNode.GameNodeGroup);
        AddToGroup(StageRoot.ProcessGroup);
        Life = InitialLife;
    }

    private async void ActivateOnScreen()
    {
        m_OnScreen = GetNodeOrNull<VisibleOnScreenNotifier2D>("OnScreen");

        if (m_OnScreen is not null && RemoveScreenExited)
        {
            _ = m_OnScreen?.Connect(VisibleOnScreenNotifier2D.SignalName.ScreenExited, new(this, MethodName.ExitScreen));
        }

        _ = await ToSignal(GetTree().CreateTimer(0.05f, false), Timer.SignalName.Timeout);
        m_AMomentWaited = true;
    }

    public virtual void CalcSpeed(float speed, float approach, float reductionApproach)
    {
        m_MaxSpeed = speed;
        m_Approach = approach;
        m_ReductionApproach = reductionApproach;
        m_Acceleration = CalcAcceleration(speed, approach);
        m_ReductionAcceleration = CalcAcceleration(speed, reductionApproach);
    }

    public virtual void InitialSpeed()
    {
        m_MaxSpeed = InitialMaxSpeed;
        m_Approach = InitialApproach;
        m_ReductionApproach = InitialReductionApproach;
        m_Acceleration = CalcAcceleration(m_MaxSpeed, m_Approach);
        m_ReductionAcceleration = CalcAcceleration(m_MaxSpeed, m_ReductionApproach);
    }

    public static float CalcAcceleration(float speed, float approach)
    {
        return Mathf.Pow(speed, 2f) / (approach * 2f);
    }

    protected virtual void PlaySe(string name)
    {
        if (m_OnScreen is null || m_OnScreen.IsOnScreen())
        {
            GetNode<SePlayer>("/root/SePlayer").Play(name);
        }
    }

    public static async void ThrowAwayNode2D(Node2D node)
    {
        if (node is null)
        {
            return;
        }

        node.SetProcess(false);
        node.GlobalPosition = new(-2000f, -2000f);
        _ = await node.ToSignal(node.GetTree().CreateTimer(0.05f, false), Timer.SignalName.Timeout);
        node.QueueFree();
    }

    #region ILifeインタフェース
    public virtual void AddLife(int value)
    {
        if (value == 0 || Life == 0 || (0 < value && Life == MaxLife))
        {
            return;
        }

        Life = Mathf.Clamp(Life + value, 0, MaxLife);

        if (value < 0)
        {
            if (Life == 0)
            {
                Dead();
            }
            else
            {
                Damaged();
            }
        }
        else
        {
            if (Life == MaxLife)
            {
                FullRecovered();
            }
            else
            {
                Recovered();
            }
        }
    }

    public virtual void FullRecovered()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("FullRecovered"), this, true);
    }

    public virtual void Recovered()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("Recovered"), this, true);
    }

    public virtual void Damaged()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("Damaged"), this, true);
    }

    public virtual void Dead()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("Dead"), this, true);
    }
    #endregion

    #region IGameNodeインタフェース
    public virtual void InitializeNode()
    {
        _ = Connect(SignalName.SceneAdded, new(GetNode<GameDialogLayer>("/root/DialogLayer").GetCurrentGameRoot(), GameStageRoot.MethodName.AddScene));
        InitialSpeed();
    }

    public virtual void ExitScreen()
    {
        if (RemoveScreenExited)
        {
            RemoveNode();
        }
    }

    public virtual void RemoveNode()
    {
        ThrowAwayNode2D(this);
    }
    #endregion
}
