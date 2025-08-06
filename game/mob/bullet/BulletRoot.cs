using Godot;
using teos.common.system;
using teos.game.mob.enemy;

namespace teos.game.mob.bullet;

/// <summary>
/// 弾の親、動かない
/// </summary>
public partial class BulletRoot : Mob, ISweep
{
    public static readonly string ParentNodeName = "Bullet";

    [Export]
    public bool Pierce { get; set; } = false;

    [Export]
    public int Attack { get; set; } = -1;

    [Export]
    public bool EnemyShot { get; set; } = false;

    [Export]
    public Color BulletModulate { get; set; } = Color.Color8(255, 255, 255);

    [Export]
    public Color BulletModulateBase { get; set; } = Color.Color8(255, 255, 255);

    private bool _blink = false;
    private bool _modulate = false;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        base._Ready();
        _collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
    }

    public virtual void HitArea2D(Area2D node)
    {
        HitNode2D(node);
    }

    public virtual void HitNode2D(Node2D node)
    {
        if (!Visible)
        {
            return;
        }

        if (node is ILife inode)
        {
            inode.AddLife(Attack);
        }

        if (!Pierce)
        {
            RemoveNode();
        }
    }

    public void Switch()
    {
        if (Visible && _collisionShape is not null)
        {
            _blink = !_blink;
            _collisionShape.Disabled = _blink;
        }
    }

    public void ModulateBlink()
    {
        _modulate = !_modulate;
        Modulate = _modulate ? BulletModulate : BulletModulateBase;
    }

    #region IGameNodeインタフェース
    public override void InitializeNode()
    {
        base.InitializeNode();

        if (EnemyShot)
        {
            AddToGroup(EnemyRoot.EnemyGroup);
            AddToGroup(GameSpeedManager.GameSpeedManageGroup);
            SetCollisionLayerValue(8, true);
        }
        else
        {
            SetCollisionLayerValue(5, true);
        }
    }
    #endregion

    #region ISweepインタフェース
    private void Sweep()
    {
        RemoveNode();
    }
    #endregion
}
