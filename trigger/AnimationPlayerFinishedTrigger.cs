using Godot;
using teos.command;

namespace teos.trigger;

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
