using Godot;

namespace maid_by_shiraishi.system;

/// <summary>
/// ゲーム設定
/// プロジェクト設定＞グローバル＞自動読み込みで自動的に実行が開始される。
/// </summary>
public partial class GameOption : Node
{
    private static readonly string OptionsFilePath = "user://options.dat";
    private static readonly float BgmDefaultVolume = 70f;
    private static readonly float SeDefaultVolume = 60f;
    private static readonly float VoiceDefaultVolume = 75f;
    private ConfigFile _audioVolumeOptions = new();

    /// <summary>
    /// BGM音量
    /// </summary>
    public float BgmVolume { get; set; } = BgmDefaultVolume;

    /// <summary>
    /// 効果音音量
    /// </summary>
    public float SeVolume { get; set; } = SeDefaultVolume;

    /// <summary>
    /// 音声音量
    /// </summary>
    public float VoiceVolume { get; set; } = VoiceDefaultVolume;

    public override void _Ready()
    {
        LoadOptions();
        SetOptions();
    }

    /// <summary>
    /// デフォルト音量にリセットする
    /// </summary>
    public void ResetOptions()
    {
        BgmVolume = BgmDefaultVolume;
        SeVolume = SeDefaultVolume;
        VoiceVolume = VoiceDefaultVolume;
    }

    /// <summary>
    /// ゲームシステム設定を保存する
    /// </summary>
    public void SaveOptions()
    {
        BgmVolume = Mathf.Clamp(BgmVolume, 0f, 100f);
        SeVolume = Mathf.Clamp(SeVolume, 0f, 100f);
        VoiceVolume = Mathf.Clamp(VoiceVolume, 0f, 100f);
        _audioVolumeOptions.SetValue("Volume", "BGM", BgmVolume);
        _audioVolumeOptions.SetValue("Volume", "SE", SeVolume);
        _audioVolumeOptions.SetValue("Volume", "VOICE", VoiceVolume);
        Error e = _audioVolumeOptions.Save(OptionsFilePath);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{OptionsFilePath}を保存できませんでした。エラーコードは{e}です。");
        }
    }

    /// <summary>
    /// 設定ファイルをまとめて読み込む
    /// </summary>
    public void LoadOptions()
    {
        _audioVolumeOptions.Clear();
        ResetOptions();
        Error e = _audioVolumeOptions.Load(OptionsFilePath);

        if (e is Error.FileNotFound)
        {
            return;
        }
        else if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{OptionsFilePath}を読み込めませんでした。エラーコードは{e}です。");
            return;
        }

        BgmVolume = (float)Mathf.Clamp(_audioVolumeOptions.GetValue("Volume", "BGM", BgmDefaultVolume).AsDouble(), 0f, 100f);
        SeVolume = (float)Mathf.Clamp(_audioVolumeOptions.GetValue("Volume", "SE", SeDefaultVolume).AsDouble(), 0f, 100f);
        VoiceVolume = (float)Mathf.Clamp(_audioVolumeOptions.GetValue("Volume", "VOICE", VoiceDefaultVolume).AsDouble(), 0f, 100f);
    }

    /// <summary>
    /// BGM音量を適用する
    /// </summary>
    public void SetOptions()
    {
        BgmVolume = Mathf.Clamp(BgmVolume, 0f, 100f);
        SeVolume = Mathf.Clamp(SeVolume, 0f, 100f);
        VoiceVolume = Mathf.Clamp(VoiceVolume, 0f, 100f);
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("BGM"), ToDecibel(BgmVolume));
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SE"), ToDecibel(SeVolume));
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("VOICE"), ToDecibel(VoiceVolume));
    }

    /// <summary>
    /// 0-100の音量をデシベルに変換する
    /// </summary>
    /// <param name="volume">音量</param>
    /// <returns>デシベル</returns>
    private static float ToDecibel(float volume)
    {
        float v = Mathf.Clamp(volume / 100f, 0f, 1f);
        float db = Mathf.LinearToDb(v);
        return db;
    }
}
