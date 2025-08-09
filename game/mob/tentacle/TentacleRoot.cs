using Godot;
using Godot.Collections;
using teos.game.stage.character_manager;

namespace teos.game.mob.tentacle;

/// <summary>
/// 触手の基点
/// </summary>
public partial class TentacleRoot : Node2D, ICharacterManager
{
    private TentacleHead _head;
    private bool _disable = false;
    private bool _disableRoot = true;
    private Node _neckRoot;

    public override void _Ready()
    {
        _head = GetNodeOrNull<TentacleHead>("TentacleHead");
        _neckRoot = GetNodeOrNull("EnemyTentacleNeck");
        AddToGroup(CharacterManager.CharacterGroup);
    }

    public override void _Process(double delta)
    {
        if (_disableRoot)
        {
            return;
        }

        if (_disable)
        {
            SweepRoot();
            return;
        }

        if (_head is null || (_head is not null && !_head.HasISweepNode()))
        {
            SweepAll();
        }
    }

    public async void SweepAll()
    {
        _disable = true;
        Array<Node> list = GetChildren();
        list.Reverse();

        foreach (Node n in list)
        {
            if (IsInstanceValid(n) && n is TentacleNeck neck)
            {
                neck.SweepAll();
                _ = await ToSignal(GetTree().CreateTimer(0.1f, false), Timer.SignalName.Timeout);
            }
        }

        if (_neckRoot is ISweep sweep)
        {
            sweep.Sweep();
        }
    }

    public void SweepRoot()
    {
        foreach (Node n in GetChildren())
        {
            if (n is TentacleNeck neck && neck.HasISweepNode())
            {
                return;
            }
        }

        _disableRoot = true;
        Mob.ThrowAwayNode2D(this);
    }

    private void ConnectTentacles()
    {
        Array<Node> children = GetChildren();
        int length = children.Count;
        bool firstNeck = true;
        TentacleNeck previous = null;

        for (int i = 0; i < length; i++)
        {
            if (children[i] is not TentacleNeck neck)
            {
                continue;
            }

            if (firstNeck)
            {
                firstNeck = false;
                neck.MobPrevious = this;
                previous = neck;
                continue;
            }

            previous.MobNext = neck;
            neck.MobPrevious = previous;
            previous = neck;
        }

        TentacleHead head = GetNode<TentacleHead>("TentacleHead");
        previous.MobNext = head;
        head.MobPrevious = previous;
        head.TentacleRoot = this;
    }

    #region ICharacterManagerインタフェース
    public void ActiveCharacter(bool active)
    {
        ConnectTentacles();
        _disableRoot = false;
    }
    #endregion
}

