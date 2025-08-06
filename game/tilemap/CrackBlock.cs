using Godot;
using Godot.Collections;
using teos.common.command;
using teos.common.system;
using teos.common.tilemap;
using teos.game.mob;
using teos.game.stage.character_manager;

namespace teos.game.tilemap;

/// <summary>
/// ひび割れブロック
/// </summary>
public partial class CrackBlock : Area2D, ILife, IItemDropper
{
    [Export]
    public int Life { get; set; } = 5;

    /// <summary>
    /// 弱点として扱う
    /// </summary>
    [Export]
    public bool WeakPoint { get; set; } = true;

    protected VisibleOnScreenNotifier2D m_OnScreen;

    private AnimatedSprite2D _animatedSprite2D;
    private TileMapManager _tilemapManager;
    private int _max;
    private Array<EnemyDropCharacterEnabler> _dropItemCharacterEnabler = [];

    public override void _Ready()
    {
        m_OnScreen = GetNodeOrNull<VisibleOnScreenNotifier2D>("OnScreen");
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _max = _animatedSprite2D.SpriteFrames.GetFrameCount("default") - 1;
        _animatedSprite2D.Frame = Mathf.Clamp(5 - Life, 0, _max);

        if (GetParent() is TileMapManager tileMapManager)
        {
            _tilemapManager = tileMapManager;
        }
    }

    public virtual void RemoveNode()
    {
        Mob.ThrowAwayNode2D(this);
    }

    protected virtual void PlaySe(string name)
    {
        if (m_OnScreen is null || m_OnScreen.IsOnScreen())
        {
            GetNode<SePlayer>("/root/SePlayer").Play(name);
        }
    }

    #region ILifeインタフェース
    public void AddLife(int value)
    {
        if ((m_OnScreen is null || m_OnScreen.IsOnScreen()) && value < 0)
        {
            Life = Mathf.Clamp(Life + value, 0, int.MaxValue);

            if (Life > 0)
            {
                Damaged();
            }
            else
            {
                Dead();
            }
        }
    }

    public void Damaged()
    {
        _animatedSprite2D.Frame = Mathf.Clamp(5 - Life, 0, _max);
        CommandRoot.ExecChildren(GetNodeOrNull("Damaged"), this, true);
    }

    public void Dead()
    {
        _animatedSprite2D.Visible = false;
        CommandRoot.ExecChildren(GetNodeOrNull("Dead"), this, true);
        SetCollisionLayerValue(12, false);
        _tilemapManager?.RemoveBlock((Vector2I)Position);
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
