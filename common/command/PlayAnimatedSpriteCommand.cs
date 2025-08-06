using Godot;

namespace teos.common.command;

/// <summary>
/// AnimatedSprite2Dのアニメーションを開始するコマンド
/// </summary>
public partial class PlayAnimatedSpriteCommand : CommandRoot
{
    /// <summary>
    /// 開始するAnimatedSprite2D
    /// </summary>
    [Export]
    public AnimatedSprite2D AnimatedSprite { get; set; }

    [Export]
    public string AnimationName { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || AnimatedSprite is null)
        {
            return;
        }

        AnimatedSprite?.Play(AnimationName);
    }
}
