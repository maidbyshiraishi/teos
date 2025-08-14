using Godot;
using Godot.Collections;
using teos.common.command;
using teos.common.path;
using teos.game.mob.player;
using teos.game.stage.character_manager;
using teos.game.weapon;

namespace teos.game.mob.enemy;

/// <summary>
/// 敵の親
/// </summary>
public partial class EnemyRoot : Fighter, ICharacterManager, ISweep, IPathFollower, IItemDropper
{
    public static readonly string EnemyGroup = "EnemyGroup";
    public static readonly string ParentNodeName = "Enemy";

    [Export]
    public int Attack { get; set; } = -1;

    [Export]
    public bool Flipped { get; set; } = false;

    [ExportGroup("Weapon")]

    [Export]
    public bool WeaponEquipEnabled { get; set; } = true;

    [Export]
    public bool WeaponRotationEnabled { get; set; } = false;

    [ExportGroup("SE")]

    [Export]
    public string DirectAttackSe { get; set; }

    protected MountPoint m_MountPoint;
    protected AnimationTree m_AnimationTree;
    protected AnimationNodeStateMachinePlayback m_StateMachine;
    protected Player m_Player;
    protected CharacterManager m_CharacterManager;
    protected Array<PathFollow> m_PathFollow;
    protected Sprite2D m_Sprite2d;
    protected Vector2 m_OldPosition;
    protected bool m_Trigger = true;

    private Array<EnemyDropCharacterEnabler> _dropItemCharacterEnabler = [];

    public override void _Ready()
    {
        m_MountPoint = GetNode<MountPoint>("MountPoint");
        m_AnimationTree = GetNode<AnimationTree>("AnimationTree");
        m_StateMachine = (AnimationNodeStateMachinePlayback)m_AnimationTree.Get("parameters/playback");
        m_PathFollow = FindPathFollow(this);
        m_Sprite2d = GetNodeOrNull<Sprite2D>("Sprite2D");

        if (m_Sprite2d is not null)
        {
            m_Sprite2d.FlipH = Flipped;
        }

        m_OldPosition = GlobalPosition;
        base._Ready();
        AddToGroup(CharacterManager.CharacterGroup);
    }

    public static Array<PathFollow> FindPathFollow(Node root)
    {
        Array<PathFollow> ret = [];
        Node now = root.GetParentOrNull<Node>();

        while (now is not null)
        {
            if (now is PathFollow pathFollow)
            {
                ret.Add(pathFollow);
            }

            now = now.GetParentOrNull<Node>();
        }

        return ret;
    }

    public static void PathFollowMove(Array<PathFollow> pathFollowArray, double delta)
    {
        if (pathFollowArray is not null)
        {
            foreach (PathFollow pathFollow in pathFollowArray)
            {
                pathFollow.ManualScroll(delta);
            }
        }
    }

    public override void EquipWeapon(WeaponRoot weapon, bool instantly)
    {
        if (!WeaponEquipEnabled || m_StateMachine.GetCurrentNode() == "terminate" || m_StateMachine.GetCurrentNode() == "dead")
        {
            return;
        }

        _ = m_MountPoint.EquipWeapon(this, weapon, true, instantly);
        base.EquipWeapon(weapon, instantly);
    }

    public override void SeparateWeapon()
    {
        m_MountPoint.SeparateWeapon(this);
        base.SeparateWeapon();
    }

    public void EnteredArea2D(Area2D area)
    {
        if (m_StateMachine.GetCurrentNode() == "idle" && area is Player player)
        {
            PlaySe(DirectAttackSe);
            player.AddLife(Attack);
        }
    }

    public void DamageControl()
    {
        if (Life == 0)
        {
            m_StateMachine.Start("dead");
        }
    }

    #region ILifeインタフェース
    public override void AddLife(int value)
    {
        if (m_StateMachine.GetCurrentNode() == "idle")
        {
            base.AddLife(value);

            if (value <= 0)
            {
                m_StateMachine.Travel("damage_control");
            }
        }
    }

    public override void Dead()
    {
        m_Trigger = false;
        base.Dead();
    }
    #endregion

    #region ICharacterManagerインタフェース
    public void SetCharacterManager(CharacterManager characterManager)
    {
        m_CharacterManager = characterManager;
    }

    public virtual void ActiveCharacter(bool active)
    {
        m_StateMachine.Start(active ? "initialize" : "sleep");
    }

    public virtual void InitializeCharacter()
    {
        AddToGroup(EnemyGroup);
        CommandRoot.ExecChildren(GetNodeOrNull("InitializeCharacter"), this, true);
    }

    public virtual void TerminateCharacter()
    {
        RemoveFromGroup(EnemyGroup);
        CommandRoot.ExecChildren(GetNodeOrNull("TerminateCharacter"), this, true);
        RemoveNode();
    }
    #endregion

    #region IGameNodeインタフェース
    public override void InitializeNode()
    {
        base.InitializeNode();
        m_Player = Player.GetPlayer(this);
    }

    public override void ExitScreen()
    {
        if (RemoveScreenExited)
        {
            TerminateCharacter();
        }
    }
    #endregion

    #region ISweepインタフェース
    public void Sweep()
    {
        if (IsInstanceValid(this))
        {
            m_StateMachine.Start("dead");
        }
    }
    #endregion

    #region IItemDropperインタフェース
    public void AddItemDropper(EnemyDropCharacterEnabler enabler)
    {
        _dropItemCharacterEnabler.Add(enabler);
    }

    public void DropItem()
    {
        foreach (EnemyDropCharacterEnabler enabler in _dropItemCharacterEnabler)
        {
            enabler.EnableCharacter();
        }
    }
    #endregion
}
