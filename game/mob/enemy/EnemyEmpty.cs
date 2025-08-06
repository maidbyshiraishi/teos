using Godot;
using Godot.Collections;
using teos.common.command;
using teos.common.path;
using teos.common.stage;
using teos.game.stage.character_manager;

namespace teos.game.mob.enemy;

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
        AddToGroup(StageRoot.ProcessGroup);
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

    public override void _Process(double delta)
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
    public void SetCharacterManager(CharacterManager characterManager)
    {
        m_CharacterManager = characterManager;
    }

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
    public virtual void RemoveNode()
    {
        Mob.ThrowAwayNode2D(this);
    }
    #endregion

    #region ISweepインタフェース
    public void Sweep()
    {
        TerminateCharacter();
    }
    #endregion
}
