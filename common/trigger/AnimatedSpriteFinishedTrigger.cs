using Godot;
using teos.common.command;

namespace teos.common.trigger;

/// <summary>
/// AnimatedSprite2D終了トリガー
/// </summary>
public partial class AnimatedSpriteFinishedTrigger : Node
{
    [Export]
    public Node Target { get; set; }

    public override void _Ready()
    {
        if (GetParent() is AnimatedSprite2D animatedSprite2d)
        {
            _ = animatedSprite2d.Connect(AnimatedSprite2D.SignalName.AnimationFinished, new(this, MethodName.Exec));
        }
    }

    public virtual void Exec()
    {
        CommandRoot.ExecChildren(this, Target, true);
    }
}
