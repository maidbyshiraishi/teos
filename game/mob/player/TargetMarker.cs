using Godot;
using teos.common.stage;
using teos.game.system;

namespace teos.game.mob.player;

public partial class TargetMarker : Node2D, IGameNode
{
    [Export]
    public float Speed { get; set; } = 600f;

    [Export]
    public ReferenceRect BorderRect { get; set; }

    private Vector2 _mouseRelative = Vector2.Zero;
    private GameKeyOption _gameKeyOption;

    public override void _Ready()
    {
        _gameKeyOption = GetNode<GameKeyOption>("/root/GameKeyOption");
        AddToGroup(IGameNode.GameNodeGroup);
    }

    public override void _Process(double delta)
    {
        Vector2 input = Input.GetVector("game_target_left", "game_target_right", "game_target_up", "game_target_down");
        Vector2 position = Position;

        if (input.Length() > 0f)
        {
            // 速度低下の倍率に応じて加速させる
            position += input * Speed * (float)delta * 1f / (float)Engine.TimeScale * (float)_gameKeyOption.TargetSensitivity;
        }
        else
        {
            position += _mouseRelative * (float)_gameKeyOption.MouseSensitivity;
            _mouseRelative = Vector2.Zero;
        }

        if (BorderRect is not null)
        {
            Vector2 begin = BorderRect.GetBegin();
            Vector2 end = BorderRect.GetEnd();
            Vector2 new_position = position.Clamp(begin, end);
            Position = new_position;
        }
        else
        {
            Position = position;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion)
        {
            _mouseRelative = motion.Relative;
        }
    }
}
