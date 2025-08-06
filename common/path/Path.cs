using Godot;
using Godot.Collections;

namespace teos.common.path;

/// <summary>
/// 終端処理を行うパス
/// </summary>
public partial class Path : Path2D
{
    private Array<PathFollow> _pathFollow = [];
    private PathFollow _parentPathFollow;

    public override void _Ready()
    {
        _parentPathFollow = SearchParentPathFollow();
        SearchPathFollow();
        SetChildrenPathLooped();
    }

    public override void _Process(double delta)
    {
        if (GetChildCount() == 0)
        {
            SetProcess(false);
            QueueFree();
        }
    }

    private PathFollow SearchParentPathFollow()
    {
        return _parentPathFollow is null ? GetParentOrNull<PathFollow>() : _parentPathFollow;
    }

    public void SearchPathFollow()
    {
        if (_pathFollow is not null)
        {
            return;
        }

        foreach (Node n in GetChildren())
        {
            if (n is PathFollow pathFollow)
            {
                _pathFollow.Add(pathFollow);
                return;
            }
        }
    }

    public void AddToPathFollow(Node node, int index = 0)
    {
        int count = _pathFollow.Count;

        if (count == 0 || index < 0)
        {
            return;
        }

        if (index < count)
        {
            _pathFollow[index].AddChild(node);
        }
    }

    private void SetChildrenPathLooped()
    {
        bool looped;
        Curve2D curve = Curve;

        if (curve is null)
        {
            looped = true;
        }
        else
        {
            int count = curve.PointCount;
            looped = count == 0 || curve.GetPointPosition(0) == curve.GetPointPosition(count - 1);
        }

        foreach (PathFollow pathFollow in _pathFollow)
        {
            pathFollow.ParentPathLooped = looped;
        }
    }

    public void SetAutoScrollAll(bool autoScroll)
    {
        SetAutoScroll(autoScroll);

        foreach (PathFollow pathFollow in _pathFollow)
        {
            pathFollow.SetAutoScrollAll(autoScroll);
        }
    }

    public void SetAutoScroll(bool autoScroll, int index = -1)
    {
        if (index < 0)
        {
            foreach (PathFollow pathFollow in _pathFollow)
            {
                pathFollow.AutoScroll = autoScroll;
            }

            return;
        }

        if (index < _pathFollow.Count)
        {
            _pathFollow[index].AutoScroll = autoScroll;
        }
    }

    public void ManualScroll(double delta)
    {
        _parentPathFollow?.ManualScroll(delta);
    }
}
