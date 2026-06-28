using Godot;
using Godot.Collections;
using maid_by_shiraishi.command;
using maid_by_shiraishi.mob;
using maid_by_shiraishi.mob.shot;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.stage.character_manager;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.weapon;

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

    [ExportGroup("Shot")]

    [Export]
    public PackedScene Shot { get; set; }

    [Export]
    public int NumOfShots { get; set; }

    [Export]
    public bool UnlimitedMode { get; set; } = false;

    [ExportGroup("Enemy")]

    [Export]
    public bool EnemyEquiped { get; set; }

    [Export]
    public bool HasEnemyStatemachine { get; set; } = false;

    protected AnimationNodeStateMachinePlayback m_StateMachine;
    protected uint m_ShotTargetLayer;
    protected Color m_ShotModulate;

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
        _animationTree.Set("parameters/conditions/empty", NumOfShots <= 0);
    }

    public virtual void Fire()
    {
        if (Shot is null || NumOfShots <= 0 || !_equipped)
        {
            return;
        }

        foreach (Node node in _muzzle)
        {
            if (NumOfShots <= 0)
            {
                break;
            }

            if (node is not Marker2D marker)
            {
                continue;
            }

            if (Shot.Instantiate() is ShotRoot shot)
            {
                MakeShot(marker, shot);
            }
        }
    }

    private void MakeShot(Marker2D maker, ShotRoot shot)
    {
        if (!UnlimitedMode)
        {
            NumOfShots--;
        }

        shot.Transform = maker.GlobalTransform;
        shot.CollisionMask = m_ShotTargetLayer;
        shot.ShotModulate = m_ShotModulate;
        shot.EnemyShot = EnemyEquiped;
        _ = EmitSignal(SignalName.SceneAdded, [shot, ShotRoot.ParentNodeName]);
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
            m_ShotTargetLayer = fighter.ShotTargetLayer;
            m_ShotModulate = fighter.ShotModulate;
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

    public void PlaySe(string name) => GetNode<SePlayer>("/root/SePlayer").Play(name);

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
    public virtual void InitializeNode() => SceneAdded += GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot().AddScene;
    #endregion

    #region ICharacterManagerインタフェース
    public void SetCharacterManager(CharacterManager characterManager) => _characterManager = characterManager;

    public void ActiveCharacter(bool active)
    {
        _animationTree.Set("parameters/conditions/press", false);
        _animationTree.Set("parameters/conditions/release", false);
        _animationTree.Set("parameters/conditions/empty", NumOfShots <= 0);

        if (active && GetParent() is Fighter fighter)
        {
            fighter.EquipWeapon(this, true);
            return;
        }

        m_StateMachine.Travel(active ? "initialize" : "sleep");
    }

    public void InitializeCharacter() => CommandRoot.ExecChildren(GetNodeOrNull("InitializeCharacter"), this, true);

    public void TerminateCharacter() => CommandRoot.ExecChildren(GetNodeOrNull("TerminateCharacter"), this, true);
    #endregion
}
