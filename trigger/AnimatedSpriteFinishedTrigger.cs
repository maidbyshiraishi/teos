using Godot;
using maid_by_shiraishi.command;

namespace maid_by_shiraishi.trigger;

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
            animatedSprite2d.AnimationFinished += Exec;
        }
    }

    public virtual void Exec() => CommandRoot.ExecChildren(this, Target, true);
}
