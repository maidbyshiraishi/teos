using Godot;
using teos.common.command;

namespace teos.common.path;

/// <summary>
/// パスに沿って動くもの
/// </summary>
public partial class PathFollow : PathFollow2D
{
    [Export]
    public bool AutoScroll { get; set; } = false;

    [Export]
    public float Speed { get; set; }

    [ExportGroup("Path")]

    [Export]
    public PathEndType PathEndType { get; set; } = PathEndType.Shuttle;

    [Export]
    public bool Reverse { get; set; } = false;

    [Export]
    public bool ParentPathLooped { get; set; }

    private Path _parentPath;
    private bool _reachedToEdge = false;
    private bool _firstTimeMove = true;

    public override void _Ready()
    {
        if (GetParent() is Path path)
        {
            _parentPath = path;
        }
    }

    public override void _Process(double delta)
    {
        if (GetChildCount() == 0)
        {
            SetProcess(false);
            QueueFree();
            return;
        }

        if (AutoScroll)
        {
            Scroll(delta);
        }
    }

    public void ManualScroll(double delta)
    {
        if (AutoScroll || _parentPath is null || _parentPath.Curve is null)
        {
            return;
        }

        Scroll(delta);
    }

    private void Scroll(double delta)
    {
        _parentPath.ManualScroll(delta);

        if (_firstTimeMove)
        {
            _firstTimeMove = false;
            ReachedToEdge();
        }

        Progress = Mathf.Clamp(Progress + ((Reverse ? -1f : 1f) * Speed * (float)delta), 0, _parentPath.Curve.GetBakedLength());

        if (Progress is > (-1f) and < 1.0f)
        {
            _reachedToEdge = false;
        }

        switch (PathEndType)
        {
            case PathEndType.Oneway:

                ExecOneway();
                break;

            case PathEndType.Shuttle:

                ExecShuttle();
                break;

            case PathEndType.Loop:

                ExecLoop();
                break;
        }
    }

    protected virtual void ExecOneway()
    {
        if (ProgressRatio is 1.0f or 0.0f)
        {
            ReachedToEdge();
        }
    }

    protected virtual void ExecShuttle()
    {
        if (ProgressRatio is 1.0f or 0.0f)
        {
            Reverse = !Reverse;
            ReachedToEdge();
        }
    }

    protected virtual void ExecLoop()
    {
        if (ProgressRatio == 0.0f)
        {
            ProgressRatio = 1.0f;
            ReachedToEdge();
        }
        else if (ProgressRatio == 1.0f)
        {
            ProgressRatio = 0.0f;
            ReachedToEdge();
        }
    }

    protected virtual void ReachedToEdge()
    {
        if (_reachedToEdge)
        {
            return;
        }

        _reachedToEdge = true;
        CommandRoot.ExecChildren(GetNodeOrNull("ReachedToEdge"), this, true);

        foreach (Node n in GetChildren())
        {
            if (n is IPathFollower pfollower)
            {
                pfollower.ReachedToEdge(ProgressRatio, Reverse, PathEndType);
            }
        }
    }

    public void SetSpeed(float speed)
    {
        SetDeferred("Speed", speed);
    }

    public void SetAutoScrollAll(bool autoScroll)
    {
        foreach (Node n in GetChildren())
        {
            if (n is Path path)
            {
                path.SetAutoScrollAll(autoScroll);
            }
        }
    }
}
