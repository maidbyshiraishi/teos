using Godot;
using teos.common.command;
using teos.common.stage;
using teos.game.mob;

namespace teos.game.item;

/// <summary>
/// アイテムの親
/// </summary>
public partial class ItemRoot : Area2D, IGameNode, IItem
{
    [Export]
    public bool RemoveScreenExited { get; set; } = true;

    [Export]
    public float MaxSpeed { get; set; } = 200f;

    [Export]
    public float LerpAngle { get; set; } = 30f;

    protected VisibleOnScreenNotifier2D m_OnScreen;
    protected bool m_AMomentWaited = false;

    private bool _active = false;
    private bool _blink = false;
    private CollisionShape2D _collisionShape;
    private HomingFunction _homing;

    public override void _Ready()
    {
        ActivateOnScreen();
        _collisionShape = GetNodeOrNull<CollisionShape2D>("ApproachPlayer/CollisionShape2D");
        AddToGroup(IGameNode.GameNodeGroup);
        AddToGroup(StageRoot.ProcessGroup);
        _homing = new(this, LerpAngle);
    }

    private async void ActivateOnScreen()
    {
        m_OnScreen = GetNodeOrNull<VisibleOnScreenNotifier2D>("OnScreen");

        if (m_OnScreen is not null && RemoveScreenExited)
        {
            _ = m_OnScreen?.Connect(VisibleOnScreenNotifier2D.SignalName.ScreenExited, new(this, Mob.MethodName.ExitScreen));
        }

        _ = await ToSignal(GetTree().CreateTimer(0.05f, false), Timer.SignalName.Timeout);
        m_AMomentWaited = true;
    }

    public override void _Process(double delta)
    {
        if (!_active)
        {
            return;
        }

        if (_homing.HasTarget())
        {
            _homing.Homing(delta, MaxSpeed, MaxSpeed);

        }
        else if (_active)
        {
            _homing.MoveOnly(delta, MaxSpeed, MaxSpeed);
        }

        if (m_AMomentWaited && m_OnScreen is not null && !m_OnScreen.IsOnScreen())
        {
            ExitScreen();
        }
    }

    public void Switch()
    {
        if (_collisionShape is not null)
        {
            _blink = !_blink;
            _collisionShape.Disabled = _blink;
        }
    }

    public void SetDirection(float radian)
    {
        Rotation = radian;
        _active = true;
    }

    public void EnterArea2D(Area2D node)
    {
        _homing.SetTarget(node.GlobalPosition);
        _homing.StartHoming();
        _active = true;
    }

    public void ExitArea2D(Area2D node)
    {
        _homing.ClearTarget();
        _homing.StopHoming();
    }

    #region IGameNodeインタフェース

    public void ExitScreen()
    {
        RemoveNode();
    }

    public void RemoveNode()
    {
        Mob.ThrowAwayNode2D(this);
    }
    #endregion

    #region IItemインタフェース
    public void ExecItem(Area2D node)
    {
        CommandRoot.ExecChildren(this, node, true);
    }
    #endregion
}
