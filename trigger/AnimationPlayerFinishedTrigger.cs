using Godot;
using maid_by_shiraishi.command;

namespace maid_by_shiraishi.trigger;

/// <summary>
/// AnimationPlayer終了トリガー
/// </summary>
public partial class AnimationPlayerFinishedTrigger : Node
{
    [Export]
    public Node Target { get; set; }

    public override void _Ready()
    {
        if (GetParent() is AnimationPlayer animationPlayer)
        {
            animationPlayer.AnimationFinished += Exec;
        }
    }

    public virtual void Exec(StringName animName) => CommandRoot.ExecChildren(this, Target, true);
}
