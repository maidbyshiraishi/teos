using Godot;
using teos.common.screen;

namespace teos.common.theater;

/// <summary>
/// スクロールシアターダイアログ
/// </summary>
public partial class ScrollTheater : DialogRoot
{
    public static readonly string ScrollTheaterContent = "ScrollTheaterContent";

    [Export]
    public float ScrollTime { get; set; } = 10f;

    private Control _content;
    private float _scrollSpeed = 0.1f;
    private PathFollow2D _socket;
    private Path2D _path;
    private bool _running = false;

    public override void _Ready()
    {
        _path = GetNode<Path2D>("Path2D");
        _socket = _path.GetNode<PathFollow2D>("Socket");
        _content = GetNode<Control>("Path2D/Socket/Contents");
        GetTree().CallGroup(ScrollTheaterContent, "InitializeScrollTheaterContent", [this]);
    }

    public override void InitializeNode()
    {
        Vector2 screenSize = GetViewport().GetVisibleRect().Size;
        Vector2 scrollSize = _content.GetRect().Size;
        _path.Curve.SetPointPosition(0, new(screenSize.X / 2f, screenSize.Y));
        _path.Curve.SetPointPosition(1, new(screenSize.X / 2f, -scrollSize.Y));
        _scrollSpeed = 1f / ScrollTime;
        _running = true;
    }

    public override void _Process(double delta)
    {
        if (_running && _socket is not null && _content is not null)
        {
            float speed = (float)delta * _scrollSpeed;
            _socket.ProgressRatio = Mathf.Clamp(_socket.ProgressRatio + speed, 0, 1.0f);
        }
    }

    public override void GetArgument()
    {
        GetDialogArgument("ScrollTheater");
    }

    public void SetScroll(bool running)
    {
        _running = running;
    }
}
