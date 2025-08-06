using Godot;
using teos.common.command;
using teos.game.decoration;

namespace teos.game.command;

/// <summary>
/// デコレーションを表示するコマンド
/// </summary>
public partial class ShowDecorationCommand : CommandRoot
{
    /// <summary>
    /// 表示するデコレーション
    /// </summary>
    [Export]
    public string Path { get; set; }

    [ExportGroup("RandomPosiotion")]

    /// <summary>
    /// 生成位置を乱数で補正するか
    /// </summary>
    [Export]
    public bool RandomPosiotion { get; set; } = false;

    /// <summary>
    /// 生成位置X軸の乱数補正幅
    /// </summary>
    [Export]
    public float RandomPosiotionX { get; set; } = 30f;

    /// <summary>
    /// 生成位置Y軸の乱数補正幅
    /// </summary>
    [Export]
    public float RandomPosiotionY { get; set; } = 30f;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || string.IsNullOrWhiteSpace(Path))
        {
            return;
        }

        if (node is Node2D node2d && Lib.GetPackedScene(Path) is PackedScene pack && pack.Instantiate() is Decoration decoration)
        {
            AddNode(node2d, decoration);
        }
    }

    protected void AddNode(Node2D node2d, Node2D decoration)
    {
        decoration.Position = node2d.Position;

        if (RandomPosiotion)
        {
            RandomNumberGenerator random = new();
            decoration.Position += new Vector2(random.RandfRange(-RandomPosiotionX, RandomPosiotionX), random.RandfRange(-RandomPosiotionY, RandomPosiotionY));
        }

        _ = node2d.GetParent().CallDeferred(Node.MethodName.AddChild, [decoration]);
    }
}
