using Godot;

namespace teos.common.command;

/// <summary>
/// AnimationPlayerのアニメーションを開始するコマンド
/// </summary>
public partial class PlayAnimationPlayerCommand : CommandRoot
{
    /// <summary>
    /// 開始するAnimationPlayer
    /// </summary>
    [Export]
    public AnimationPlayer AnimationPlayer { get; set; }

    /// <summary>
    /// 開始するアニメーション名
    /// </summary>
    [Export]
    public string AnimationName { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || AnimationPlayer is null || string.IsNullOrWhiteSpace(AnimationName))
        {
            return;
        }

        AnimationPlayer?.Play(AnimationName);
    }
}
