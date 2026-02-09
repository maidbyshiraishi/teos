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
            _ = animationPlayer.Connect(AnimationMixer.SignalName.AnimationFinished, new(this, MethodName.Exec));
        }
    }

    public virtual void Exec(string animName) => CommandRoot.ExecChildren(this, Target, true);
}
