using Godot;
using Godot.Collections;
using maid_by_shiraishi.command;
using maid_by_shiraishi.path;
using maid_by_shiraishi.stage;
using maid_by_shiraishi.stage.character_manager;

namespace maid_by_shiraishi.mob.enemy;

/// <summary>
/// カラの敵
/// </summary>
public partial class EnemyEmpty : Node2D, IGameNode, ICharacterManager, ISweep, IPathFollower
{
    protected CharacterManager m_CharacterManager;
    protected Array<PathFollow> m_PathFollow;

    public override void _Ready()
    {
        m_PathFollow = FindPathFollow();
        AddToGroup(CharacterManager.CharacterGroup);
        AddToGroup(IGameNode.GameNodeGroup);
        AddToGroup(GameStageRoot.ProcessGroup);
    }

    private Array<PathFollow> FindPathFollow()
    {
        Array<PathFollow> ret = [];
        Node now = GetParentOrNull<Node>();

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

    public override void _PhysicsProcess(double delta)
    {
        if (!Visible)
        {
            return;
        }

        PathFollowMove(delta);
    }

    protected void PathFollowMove(double delta)
    {
        if (m_PathFollow is not null)
        {
            foreach (PathFollow pathFollow in m_PathFollow)
            {
                pathFollow.ManualScroll(delta);
            }
        }
    }

    #region ICharacterManagerインタフェース
    public void SetCharacterManager(CharacterManager characterManager) => m_CharacterManager = characterManager;

    public virtual void ActiveCharacter(bool active)
    {
        if (active)
        {
            InitializeCharacter();
        }
    }

    public virtual void InitializeCharacter()
    {
        AddToGroup(EnemyRoot.EnemyGroup);
        CommandRoot.ExecChildren(GetNodeOrNull("InitializeCharacter"), this, true);
    }

    public virtual void TerminateCharacter()
    {
        RemoveFromGroup(EnemyRoot.EnemyGroup);
        CommandRoot.ExecChildren(GetNodeOrNull("TerminateCharacter"), this, true);
        RemoveNode();
    }
    #endregion

    #region IGameNodeインタフェース
    public virtual void RemoveNode() => Mob.ThrowAwayNode2D(this);
    #endregion

    #region ISweepインタフェース
    public void Sweep() => TerminateCharacter();
    #endregion
}
