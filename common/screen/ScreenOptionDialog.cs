using Godot;
using teos.common.system;

namespace teos.common.screen;

/// <summary>
/// 画面設定ダイアログ
/// </summary>
public partial class ScreenOptionDialog : DialogRoot
{
    /// <summary>
    /// 全画面
    /// </summary>
    [Export]
    public bool Fullscreen { get; set; } = false;

    public override void Active()
    {
        UpdateDialogScreen();
        base.Active();
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "FullscreenCheck";
    }

    /// <summary>
    /// GUI設定値を更新する
    /// </summary>
    public override void UpdateDialogScreen()
    {
        GameOption option = GetNode<GameOption>("/root/GameOption");
        Fullscreen = option.Fullscreen;
        SetFullscreenCheck(Fullscreen);
    }

    /// <summary>
    /// ウィンドウ・フルスクリーンが切り替わった
    /// </summary>
    /// <param name="toggledOn">ウィンドウ状態</param>
    public void FullscreenChanged(bool toggledOn)
    {
        Fullscreen = toggledOn;
        GetNode<Label>("FullscreenCheckValue").Text = Fullscreen ? "ON" : "OFF";

        // ウィンドウ状態に関しては即座にシステムに反映する
        ScreenOption option = GetNode<ScreenOption>("/root/ScreenOption");
        option.Fullscreen = Fullscreen;
        option.ChangeWindowMode();
    }

    public override void _Input(InputEvent ievent)
    {
        GameOption option = GetNode<GameOption>("/root/GameOption");

        if (option.Fullscreen != GetNode<CheckButton>("Control/FullscreenCheck").ButtonPressed)
        {
            SetFullscreenCheck(option.Fullscreen);
        }
    }

    private void SetFullscreenCheck(bool flag)
    {
        GetNode<Label>("FullscreenCheckValue").Text = flag ? "ON" : "OFF";
        GetNode<CheckButton>("Control/FullscreenCheck").SetPressedNoSignal(flag);
    }

    public void ResetDefaultScreenOptions()
    {
        ScreenOption option = GetNode<ScreenOption>("/root/ScreenOption");
        option.CalcScreenOptions();
        option.ApplyScreenOptions();
    }
}
