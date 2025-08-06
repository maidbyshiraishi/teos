using Godot;
using teos.common.screen;
using teos.game.system;

namespace teos.game.screen;

/// <summary>
/// キー設定ダイアログ
/// </summary>
public partial class KeyOptionDialog : DialogRoot
{
    private Label _keyInfo;
    private bool _setMode = false;
    private string _setActionName;
    private Control _localFocus;
    private string _localInfo;
    private string _localActionName;
    private string _lastLabel;

    public override void _Ready()
    {
        UpdateDialogScreen();
    }

    public override void UpdateDialogScreen()
    {
        GameKeyOption gameKeyOption = GetNode<GameKeyOption>("/root/GameKeyOption");

        if (gameKeyOption.Reverse)
        {
            GetNode<Label>("PlayerMove").Text = "ターゲット移動";
            GetNode<Label>("TargetMove").Text = "主人公移動";
        }
        else
        {
            GetNode<Label>("PlayerMove").Text = "主人公移動";
            GetNode<Label>("TargetMove").Text = "ターゲット移動";
        }

        GetNode<Slider>("Control/TargetSensitivity").Value = Mathf.Clamp(gameKeyOption.TargetSensitivity, 0.1f, 3f);
        GetNode<Slider>("Control/MouseSensitivity").Value = Mathf.Clamp(gameKeyOption.MouseSensitivity, 0.1f, 3f);
    }

    public void SwapStick()
    {
        GameKeyOption gameKeyOption = GetNode<GameKeyOption>("/root/GameKeyOption");
        gameKeyOption.Reverse = !gameKeyOption.Reverse;
        UpdateDialogScreen();
    }

    public void TargetSensitivityValueChanged(float value)
    {
        GameKeyOption gameKeyOption = GetNode<GameKeyOption>("/root/GameKeyOption");
        gameKeyOption.TargetSensitivity = Mathf.Clamp(value, 0.1f, 3f);
    }

    public void MouseSensitivityValueChanged(float value)
    {
        GameKeyOption gameKeyOption = GetNode<GameKeyOption>("/root/GameKeyOption");
        gameKeyOption.MouseSensitivity = Mathf.Clamp(value, 0.1f, 3f);
    }

    public void ResetSensitivity()
    {
        GameKeyOption gameKeyOption = GetNode<GameKeyOption>("/root/GameKeyOption");
        gameKeyOption.TargetSensitivity = 1.0f;
        gameKeyOption.MouseSensitivity = 1.0f;
        UpdateDialogScreen();
    }
}
