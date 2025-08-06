using Godot;
using teos.common.system;

namespace teos.common.screen;

/// <summary>
/// 音量設定ダイアログ
/// </summary>
public partial class VolumeOptionDialog : DialogRoot
{
    /// <summary>
    /// BGM音量
    /// </summary>
    [Export]
    public float BgmVolume { get; set; } = 100f;

    /// <summary>
    /// 効果音音量
    /// </summary>
    [Export]
    public float SeVolume { get; set; } = 100f;

    public override void Active()
    {
        UpdateDialogScreen();
        base.Active();
    }

    protected override string GetDefaultFocusNodeName()
    {
        return "BgmSlider";
    }

    /// <summary>
    /// GUI設定値を更新する
    /// </summary>
    public override void UpdateDialogScreen()
    {
        GameOption option = GetNode<GameOption>("/root/GameOption");
        BgmVolume = option.BgmVolume;
        GetNode<Label>("BgmValue").Text = $"{BgmVolume}%";
        GetNode<HSlider>("Control/BgmSlider").SetValueNoSignal(BgmVolume);
        SeVolume = option.SeVolume;
        GetNode<Label>("SeValue").Text = $"{SeVolume}%";
        GetNode<HSlider>("Control/SeSlider").SetValueNoSignal(SeVolume);
    }

    /// <summary>
    /// BGM音量が変更された
    /// </summary>
    /// <param name="value">音量</param>
    public void BgmVolumeChanged(float value)
    {
        BgmVolume = value;
        GetNode<Label>("BgmValue").Text = $"{BgmVolume}%";
        GameOption option = GetNode<GameOption>("/root/GameOption");
        option.BgmVolume = BgmVolume;
        option.SetOptions();
    }

    /// <summary>
    /// 効果音音量が変更された
    /// </summary>
    /// <param name="value">音量</param>
    public void SeVolumeChanged(float value)
    {
        SeVolume = value;
        GetNode<Label>("SeValue").Text = $"{SeVolume}%";
        GameOption option = GetNode<GameOption>("/root/GameOption");
        option.SeVolume = SeVolume;
        option.SetOptions();
        GetNode<SePlayer>("/root/SePlayer").Play("menu_select", true);
    }
}
