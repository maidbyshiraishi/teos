using Godot;
using Godot.Collections;
using teos.common.command;
using teos.common.path;
using teos.common.stage;
using teos.common.system;
using teos.common.tilemap;
using teos.game.decoration;
using teos.game.mob;
using teos.game.mob.enemy;
using teos.game.stage;
using teos.game.stage.character_manager;
using teos.game.system;

namespace teos.game.tilemap.mob;

/// <summary>
/// タイルマップ型MOB
/// </summary>
public partial class TileMapMob : TileMapManager, IGameNode, ICharacterManager, IItemDropper
{
    [Signal]
    public delegate void SceneAddedEventHandler(Node node, string parentNodePath);

    [Export]
    public bool RemoveScreenExited { get; set; } = true;

    protected VisibleOnScreenNotifier2D m_OnScreen;
    protected bool m_AMomentWaited = false;
    protected Array<PathFollow> m_PathFollow;
    protected bool m_Active = false;

    private bool _hasWeakPoint = false;
    private Array<ulong> _weakPoints = [];
    private Array<EnemyDropCharacterEnabler> _dropItemCharacterEnabler = [];

    public override void _Ready()
    {
        ActivateOnScreen();
        m_PathFollow = EnemyRoot.FindPathFollow(this);
        AddToGroup(CharacterManager.CharacterGroup);
        AddToGroup(IGameNode.GameNodeGroup);
        AddToGroup(StageRoot.ProcessGroup);
    }

    private async void ActivateOnScreen()
    {
        m_OnScreen = GetNodeOrNull<VisibleOnScreenNotifier2D>("OnScreen");

        if (m_OnScreen is not null && RemoveScreenExited)
        {
            _ = m_OnScreen?.Connect(VisibleOnScreenNotifier2D.SignalName.ScreenExited, new(this, Mob.MethodName.ExitScreen));
        }

        _ = await ToSignal(GetTree().CreateTimer(0.05f, false), Timer.SignalName.Timeout);
        m_AMomentWaited = true;
    }

    public override void _Process(double delta)
    {
        if (!m_Active)
        {
            return;
        }

        if (Destroyed())
        {
            Dead();
            return;
        }

        if (m_AMomentWaited && m_OnScreen is not null && !m_OnScreen.IsOnScreen())
        {
            ExitScreen();
        }

        PathFollowMove(delta);
    }

    protected virtual void PathFollowMove(double delta)
    {
        if (m_Active)
        {
            EnemyRoot.PathFollowMove(m_PathFollow, delta);
        }
    }

    public virtual void AdvanceBossState(int state, int value)
    {
    }

    public bool Destroyed()
    {
        if (_hasWeakPoint)
        {
            foreach (ulong id in _weakPoints)
            {
                if (IsInstanceIdValid(id))
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public async void Dead()
    {
        if (!m_Active)
        {
            return;
        }

        m_Active = false;
        CollisionEnabled = false;
        CommandRoot.ExecChildren(GetNodeOrNull("Dead"), this, true);

        if (Lib.GetPackedScene("res://game/decoration/big_explosion.tscn") is PackedScene pack && pack.Instantiate() is Decoration)
        {
            RandomNumberGenerator random = new();
            Rect2I rect = GetUsedRect();
            Vector2I tileSetSize = TileSet.TileSize;

            for (int i = 0; i < 10; i++)
            {
                Decoration decoration = pack.Instantiate() as Decoration;
                decoration.Position = GlobalPosition + new Vector2(random.RandiRange(rect.Position.X, rect.End.X) * tileSetSize.X, random.RandiRange(rect.Position.Y, rect.End.Y) * tileSetSize.Y);
                _ = EmitSignal(SignalName.SceneAdded, [decoration, Decoration.ParentNodeName]);
                _ = await ToSignal(GetTree().CreateTimer(0.1f, false), Timer.SignalName.Timeout);
            }

            Visible = false;
        }

        TerminateCharacter();
    }

    protected virtual void PlaySe(string name)
    {
        if (m_OnScreen is null || m_OnScreen.IsOnScreen())
        {
            GetNode<SePlayer>("/root/SePlayer").Play(name);
        }
    }

    protected void FindWeakPoint()
    {
        foreach (Node n in GetChildren())
        {
            if (n is CrackBlock crackBlock && IsInstanceValid(crackBlock) && crackBlock.WeakPoint)
            {
                _hasWeakPoint = true;
                _weakPoints.Add(crackBlock.GetInstanceId());
            }
        }
    }

    #region IGameNodeインタフェース
    public virtual void InitializeNode()
    {
        _ = Connect(SignalName.SceneAdded, new(GetNode<GameDialogLayer>("/root/DialogLayer").GetCurrentGameRoot(), GameStageRoot.MethodName.AddScene));
    }

    public virtual void ExitScreen()
    {
        if (RemoveScreenExited)
        {
            TerminateCharacter();
        }
    }

    public virtual void RemoveNode()
    {
        Mob.ThrowAwayNode2D(this);
    }
    #endregion

    #region ICharacterManagerインタフェース
    public virtual void ActiveCharacter(bool active)
    {
        m_Active = active;
        CollisionEnabled = true;
        FindWeakPoint();
    }

    public virtual void InitializeCharacter()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("InitializeCharacter"), this, true);
    }

    public virtual void TerminateCharacter()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("TerminateCharacter"), this, true);
        RemoveNode();
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
