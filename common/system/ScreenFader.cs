using Godot;

namespace teos.common.system;

/// <summary>
/// 画面遷移エフェクト制御
/// プロジェクト設定＞グローバル＞自動読み込みで自動的に実行が開始される。
/// アニメーションの初期値としてnoneを指定しなければ、スプラッシュスクリーンが表示されない場合がある。
/// </summary>
public partial class ScreenFader : CanvasLayer
{
    [Signal]
    public delegate void ScreenFadeFinishedEventHandler();

    public void ScreenFade(string effectName)
    {
        if (GetNodeOrNull("AnimatedSprite2D") is AnimatedSprite2D fader && !string.IsNullOrWhiteSpace(effectName) && fader.SpriteFrames.HasAnimation(effectName))
        {
            StartFader(fader, effectName);
            return;
        }

        WaitProcessFrame();
    }

    private async void StartFader(AnimatedSprite2D fader, string effectName)
    {
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        fader.Play(effectName);
    }

    private async void WaitProcessFrame()
    {
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        AnimationFinished();
    }

    public void AnimationFinished()
    {
        _ = EmitSignal(SignalName.ScreenFadeFinished);
    }
}
