using Godot;
using teos.common.command;

namespace teos.common.trigger;

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
            _ = animatedSprite2d.Connect(AnimatedSprite2D.SignalName.FrameChanged, new(this, MethodName.CountUp));
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
