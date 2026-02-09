using Godot;
using teos.system;

namespace teos.screen;

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

    public override void _Ready()
    {
        base._Ready();

        // Godotエディタからシグナルを接続すると
        // リリースビルドのエクスポート時、接続が失われることがある。
        _ = GetNodeOrNull<CheckButton>("Control/FullscreenCheck")?.Connect(BaseButton.SignalName.Toggled, new(this, MethodName.FullscreenChanged));
        _ = GetNodeOrNull<Button>("Control/Reset")?.Connect(BaseButton.SignalName.Pressed, new(this, MethodName.ResetDefaultScreenOptions));
    }

    public override void Active()
    {
        UpdateDialogScreen();
        base.Active();
    }

    protected override string GetDefaultFocusNodeName() => "FullscreenCheck";

    /// <summary>
    /// GUI設定値を更新する
    /// </summary>
    public override void UpdateDialogScreen()
    {
        ScreenOption option = GetNode<ScreenOption>("/root/ScreenOption");
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
