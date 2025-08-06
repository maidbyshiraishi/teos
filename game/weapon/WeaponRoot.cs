using Godot;
using Godot.Collections;
using teos.common.command;
using teos.common.stage;
using teos.common.system;
using teos.game.mob;
using teos.game.mob.bullet;
using teos.game.stage;
using teos.game.stage.character_manager;
using teos.game.system;

namespace teos.game.weapon;

/// <summary>
/// 武器の親
/// </summary>
public partial class WeaponRoot : Area2D, IGameNode, ICharacterManager
{
    public static readonly string ParentNodeName = "Item";

    [Signal]
    public delegate void SceneAddedEventHandler(Node node, string parentNodePath);

    [Export]
    public bool RotationEnabled { get; set; } = true;

    [Export]
    public string EquipSe { get; set; }

    [ExportGroup("Speed")]

    [Export]
    public float Approach { get; set; } = 96f;

    /// <summary>
    /// 最大速度
    /// </summary>
    [Export]
    public float MaxSpeed { get; set; } = 350f;

    /// <summary>
    /// 減速に要する距離
    /// </summary>
    [Export]
    public float ReductionApproach { get; set; } = 64f;

    [Export]
    public float AutoScrollSpeed { get; set; } = 100f;

    [ExportGroup("Bullet")]

    [Export]
    public PackedScene Bullet { get; set; }

    [Export]
    public int NumOfBullets { get; set; }

    [Export]
    public bool UnlimitedMode { get; set; } = false;

    [ExportGroup("Enemy")]

    [Export]
    public bool EnemyEquiped { get; set; }

    [Export]
    public bool HasEnemyStatemachine { get; set; } = false;

    protected AnimationNodeStateMachinePlayback m_StateMachine;
    protected uint m_BulletTargetLayer;
    protected Color m_BulletModulate;

    private AnimationTree _animationTree;
    private CharacterManager _characterManager;
    private Array<Marker2D> _muzzle;
    private Mutex _mutex = new();
    private bool _equipped = false;

    public override void _Ready()
    {
        // WeaponRootは画面外に出た場合でもシーンから除外されないためVisibleOnScreenNotifier2Dを持たない
        _animationTree = GetNode<AnimationTree>("AnimationTree");
        m_StateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
        _muzzle = FindMuzzle(GetNodeOrNull("Muzzle"));
        AddToGroup(CharacterManager.CharacterGroup);
        AddToGroup(IGameNode.GameNodeGroup);
    }

    public virtual void Update(bool pressA)
    {
        _animationTree.Set("parameters/conditions/press", pressA);
        _animationTree.Set("parameters/conditions/release", !pressA);
        _animationTree.Set("parameters/conditions/empty", NumOfBullets <= 0);
    }

    public virtual void Fire()
    {
        if (Bullet is null || NumOfBullets <= 0 || !_equipped)
        {
            return;
        }

        foreach (Node node in _muzzle)
        {
            if (NumOfBullets <= 0)
            {
                break;
            }

            if (node is not Marker2D marker)
            {
                continue;
            }

            if (Bullet.Instantiate() is BulletRoot bullet)
            {
                MakeBullet(marker, bullet);
            }
        }
    }

    private void MakeBullet(Marker2D maker, BulletRoot bullet)
    {
        if (!UnlimitedMode)
        {
            NumOfBullets--;
        }

        bullet.Transform = maker.GlobalTransform;
        bullet.CollisionMask = m_BulletTargetLayer;
        bullet.BulletModulate = m_BulletModulate;
        bullet.EnemyShot = EnemyEquiped;
        _ = EmitSignal(SignalName.SceneAdded, [bullet, BulletRoot.ParentNodeName]);
    }

    public virtual bool Equip(Fighter fighter, MountPoint mountPoint, bool enemy, bool instantly)
    {
        if (!instantly && fighter.PlaySeEquipWeapon)
        {
            PlaySe(EquipSe);
        }

        _mutex.Lock();
        bool result = false;

        if (!_equipped && (m_StateMachine.GetCurrentNode() == "item" || instantly))
        {
            _equipped = true;
            Reparent(mountPoint);
            mountPoint.Rotation = RotationEnabled ? Rotation : fighter.DefaultDirection.Angle();
            Rotation = 0f;
            Position = Vector2.Zero;
            EnemyEquiped = enemy;
            m_BulletTargetLayer = fighter.BulletTargetLayer;
            m_BulletModulate = fighter.BulletModulate;
            fighter.CalcSpeed(MaxSpeed, Approach, ReductionApproach);
            fighter.UpdateAutoScrollSpeed(AutoScrollSpeed);
            m_StateMachine.Start(enemy && HasEnemyStatemachine ? "enemy_equip" : "equip");
            result = true;
        }

        _mutex.Unlock();
        return result;
    }

    public virtual void Separate(Fighter fighter, MountPoint mountPoint)
    {
        _mutex.Lock();

        if (_equipped)
        {
            UnlimitedMode = false;
            Rotation = mountPoint.Rotation;
            mountPoint.Rotation = 0f;
            fighter.InitialSpeed();
            fighter.UpdateAutoScrollSpeed(null);
            m_StateMachine.Start("separate");
            _equipped = false;
        }

        _mutex.Unlock();
    }

    public void PlaySe(string name)
    {
        GetNode<SePlayer>("/root/SePlayer").Play(name);
    }

    public static Array<Marker2D> FindMuzzle(Node root)
    {
        if (root is null)
        {
            return [];
        }

        Array<Marker2D> muzzle = [];

        foreach (Node n in root.GetChildren())
        {
            if (n is Marker2D marker)
            {
                muzzle.Add(marker);
            }
        }

        return muzzle;
    }

    #region IGameNodeインタフェース
    public virtual void InitializeNode()
    {
        _ = Connect(SignalName.SceneAdded, new(GetNode<GameDialogLayer>("/root/DialogLayer").GetCurrentGameRoot(), GameStageRoot.MethodName.AddScene));
    }
    #endregion

    #region ICharacterManagerインタフェース
    public void SetCharacterManager(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public void ActiveCharacter(bool active)
    {
        _animationTree.Set("parameters/conditions/press", false);
        _animationTree.Set("parameters/conditions/release", false);
        _animationTree.Set("parameters/conditions/empty", NumOfBullets <= 0);

        if (active && GetParent() is Fighter fighter)
        {
            fighter.EquipWeapon(this, true);
            return;
        }

        m_StateMachine.Travel(active ? "initialize" : "sleep");
    }

    public void InitializeCharacter()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("InitializeCharacter"), this, true);
    }

    public void TerminateCharacter()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("TerminateCharacter"), this, true);
    }
    #endregion
}
