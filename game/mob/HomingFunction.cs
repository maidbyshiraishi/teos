using Godot;
using Godot.Collections;
using teos.game.mob.enemy;
using teos.game.mob.player;

namespace teos.game.mob;

/// <summary>
/// ホーミング弾
/// </summary>
public partial class HomingFunction(Node2D controlTarget, float lerpAngle)
{
    public Node2D ControlTarget = controlTarget;

    public float LerpAngle = lerpAngle;

    private Vector2 TargetPosition { get; set; } = Vector2.Inf;

    private bool _active = true;
    private float _speed = 0f;

    public void Homing(double delta, float maxSpeed, float acceleration)
    {
        if (_active && TargetPosition != Vector2.Inf)
        {
            ControlTarget.Rotation = (float)Mathf.LerpAngle(ControlTarget.Rotation, (TargetPosition - ControlTarget.GlobalPosition).Angle(), LerpAngle * delta);
        }

        _speed = MoveToward(ControlTarget, _speed, maxSpeed, acceleration, delta);
    }

    public void MoveOnly(double delta, float maxSpeed, float acceleration)
    {
        _speed = MoveToward(ControlTarget, _speed, maxSpeed, acceleration, delta);
    }

    public Mob FindTarget(bool searchEnemy, bool searchPlayer)
    {
        if (searchEnemy && searchPlayer)
        {
            RandomNumberGenerator random = new();
            int no = random.RandiRange(0, 1);
            return no == 0 ? Player.GetPlayer(ControlTarget) : GetEnemy();
        }
        else if (searchEnemy)
        {
            return GetEnemy();
        }
        else if (searchPlayer)
        {
            return Player.GetPlayer(ControlTarget);
        }

        return null;
    }

    private Mob GetEnemy()
    {
        Array<Node> group = ControlTarget.GetTree().GetNodesInGroup(EnemyRoot.EnemyGroup);
        int count = group.Count;

        if (count > 0)
        {
            RandomNumberGenerator random = new();
            int no = random.RandiRange(0, count - 1);

            if (group[no] is Mob mob && GodotObject.IsInstanceValid(mob))
            {
                return mob;
            }
        }

        return null;
    }

    public void StopHoming()
    {
        _active = false;
    }

    public void StartHoming()
    {
        _active = true;
    }

    public void ClearTarget()
    {
        TargetPosition = Vector2.Inf;
    }

    public void SetTarget(Vector2 target)
    {
        TargetPosition = target;
    }

    public bool HasTarget()
    {
        return TargetPosition != Vector2.Inf;
    }

    public static float MoveToward(Node2D node, float speed, float maxSpeed, float acceleration, double delta)
    {
        Vector2 position = node.Position;
        float newSpeed = Mathf.MoveToward(speed, maxSpeed, acceleration * (float)delta);
        position += node.Transform.X * newSpeed * (float)delta;
        node.Position = position;
        return newSpeed;
    }
}
