using Godot;
using maid_by_shiraishi.mob.enemy;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.mob.shot;

/// <summary>
/// 弾の親、動かない
/// </summary>
public partial class ShotRoot : Mob, ISweep
{
    public static readonly string ParentNodeName = "Shot";

    [Export]
    public bool Pierce { get; set; } = false;

    [Export]
    public int Attack { get; set; } = -1;

    [Export]
    public bool EnemyShot { get; set; } = false;

    [Export]
    public Color ShotModulate { get; set; } = Color.Color8(255, 255, 255);

    [Export]
    public Color ShotModulateBase { get; set; } = Color.Color8(255, 255, 255);

    private bool _blink = false;
    private bool _modulate = false;
    private CollisionShape2D _collisionShape;

    public override void _Ready()
    {
        base._Ready();

        // Godotエディタからシグナルを接続すると
        // リリースビルドのエクスポート時、接続が失われることがある。
        AreaEntered += HitArea2D;
        BodyEntered += HitNode2D;

        if (GetNodeOrNull("ModulateTimer") is Timer timer)
        {
            timer.Timeout += ModulateBlink;
        }

        _collisionShape = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
    }

    public virtual void HitArea2D(Area2D node) => HitNode2D(node);

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
        Modulate = _modulate ? ShotModulate : ShotModulateBase;
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
    private void Sweep() => RemoveNode();
    #endregion
}
