using Godot;
using teos.command;

namespace teos.trigger;

/// <summary>
/// アニメーションの指定フレームでコマンドを実行するトリガー
/// </summary>
public partial class AnimationFrameCountTrigger : Node
{
    /// <summary>
    /// コマンドを実行するフレーム
    /// </summary>
    [Export]
    public int FrameCount { get; set; }

    [Export]
    public Node Target { get; set; }

    private int _now = 0;

    public override void _Ready()
    {
        if (GetParent() is AnimatedSprite2D animatedSprite2d)
        {
            animatedSprite2d.FrameChanged += CountUp;
        }
    }

    private void CountUp()
    {
        _now++;

        if (_now == FrameCount)
        {
            CommandRoot.ExecChildren(this, Target, true);
        }
    }
}
