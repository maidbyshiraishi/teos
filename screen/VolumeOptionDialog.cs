using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.screen;

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

    /// <summary>
    /// 音声音量
    /// </summary>
    [Export]
    public float VoiceVolume { get; set; } = 100f;

    public override void _Ready()
    {
        base._Ready();
        GetNode<HSlider>("Control/BgmSlider").ValueChanged += BgmVolumeChanged;
        GetNode<HSlider>("Control/SeSlider").ValueChanged += SeVolumeChanged;
        GetNode<HSlider>("Control/VoiceSlider").ValueChanged += VoiceVolumeChanged;
    }

    public override void Active()
    {
        UpdateDialogScreen();
        base.Active();
    }

    protected override string GetDefaultFocusNodeName() => "BgmSlider";

    /// <summary>
    /// GUI設定値を更新する
    /// </summary>
    public override void UpdateDialogScreen()
    {
        GameOption option = SystemHub.GetGameOption();
        BgmVolume = option.BgmVolume;
        GetNode<Label>("BgmValue").Text = $"{BgmVolume}%";
        GetNode<HSlider>("Control/BgmSlider").SetValueNoSignal(BgmVolume);
        SeVolume = option.SeVolume;
        GetNode<Label>("SeValue").Text = $"{SeVolume}%";
        GetNode<HSlider>("Control/SeSlider").SetValueNoSignal(SeVolume);
        VoiceVolume = option.VoiceVolume;
        GetNode<Label>("VoiceValue").Text = $"{VoiceVolume}%";
        GetNode<HSlider>("Control/VoiceSlider").SetValueNoSignal(VoiceVolume);
    }

    /// <summary>
    /// BGM音量が変更された
    /// </summary>
    /// <param name="value">音量</param>
    public void BgmVolumeChanged(double value)
    {
        BgmVolume = (float)value;
        GetNode<Label>("BgmValue").Text = $"{BgmVolume}%";
        GameOption option = SystemHub.GetGameOption();
        option.BgmVolume = BgmVolume;
        option.SetOptions();
    }

    /// <summary>
    /// 効果音音量が変更された
    /// </summary>
    /// <param name="value">音量</param>
    public void SeVolumeChanged(double value)
    {
        SeVolume = (float)value;
        GetNode<Label>("SeValue").Text = $"{SeVolume}%";
        GameOption option = SystemHub.GetGameOption();
        option.SeVolume = SeVolume;
        option.SetOptions();
        SystemHub.GetSePlayer().Play("menu_select", true);
    }

    /// <summary>
    /// 音声音量が変更された
    /// </summary>
    /// <param name="value">音量</param>
    public void VoiceVolumeChanged(double value)
    {
        VoiceVolume = (float)value;
        GetNode<Label>("VoiceValue").Text = $"{VoiceVolume}%";
        GameOption option = SystemHub.GetGameOption();
        option.VoiceVolume = VoiceVolume;
        option.SetOptions();
        SystemHub.GetSePlayer().Play("player_recovered", false, true);
    }
}
