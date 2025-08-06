using Godot;
using teos.game.decoration;

namespace teos.game.mob.bullet;

/// <summary>
/// 炸裂弾
/// </summary>
public partial class ExplosiveBullet : BulletRoot
{
    [Export]
    public PackedScene Bullet { get; set; }

    private float _speed = 0f;
    private Area2D _explosiveArea2D;
    private uint _targetLayerBackup;

    public override void _Process(double delta)
    {
        _speed = HomingFunction.MoveToward(this, _speed, m_MaxSpeed, m_Acceleration, delta);

        if (m_AMomentWaited && RemoveScreenExited && m_OnScreen is not null && !m_OnScreen.IsOnScreen())
        {
            ExitScreen();
        }
    }

    public override void HitNode2D(Node2D node)
    {
        if (!Visible)
        {
            return;
        }

        Visible = false;

        if (Lib.GetPackedScene("res://game/decoration/explosion.tscn") is PackedScene pack && pack.Instantiate() is Decoration)
        {
            Decoration decoration = pack.Instantiate() as Decoration;
            decoration.Position = GlobalPosition;
            _ = EmitSignal(Mob.SignalName.SceneAdded, [decoration, Decoration.ParentNodeName]);
        }

        if (node is BulletRoot bullet && !bullet.Pierce)
        {
            bullet.RemoveNode();
        }

        base.HitNode2D(node);
    }

    public void HitExplosiveArea2D(Area2D node)
    {
        HitExplosiveNode2D(node);
    }

    public void HitExplosiveNode2D(Node2D node)
    {
        if (!Visible)
        {
            return;
        }

        Visible = false;

        if (Lib.GetPackedScene("res://game/decoration/explosion.tscn") is PackedScene pack && pack.Instantiate() is Decoration)
        {
            Decoration decoration = pack.Instantiate() as Decoration;
            decoration.Position = GlobalPosition;
            _ = EmitSignal(Mob.SignalName.SceneAdded, [decoration, Decoration.ParentNodeName]);
        }

        for (int i = 0; i < 360; i += 30)
        {
            if (Bullet.Instantiate() is BulletRoot bullet)
            {
                bullet.Transform = GlobalTransform;
                bullet.Rotate(Mathf.DegToRad(i));
                bullet.CollisionMask = _targetLayerBackup;
                bullet.EnemyShot = EnemyShot;
                _ = EmitSignal(Mob.SignalName.SceneAdded, [bullet, ParentNodeName]);
            }
        }

        RemoveNode();
    }

    #region IGameNodeインタフェース
    public override void InitializeNode()
    {
        base.InitializeNode();
        _targetLayerBackup = CollisionMask;
        // 5:player_shot, 8:enemy_shot
        SetCollisionMaskValue(EnemyShot ? 5 : 8, true);

        if (GetNodeOrNull("ExplosiveArea2D") is Area2D explosiveArea2D)
        {
            _explosiveArea2D = explosiveArea2D;
            // 4:player, 6:enemy, 12:target_area
            _explosiveArea2D.SetCollisionMaskValue(EnemyShot ? 4 : 6, true);
            _explosiveArea2D.SetCollisionMaskValue(12, !EnemyShot);
        }

    }
    #endregion
}
