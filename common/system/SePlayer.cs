using Godot;
using Godot.Collections;

namespace teos.common.system;

/// <summary>
/// SEの制御を行う
/// プロジェクト設定＞グローバル＞自動読み込みで自動的に実行が開始される。
/// </summary>
public partial class SePlayer : Node
{
    [Export]
    public Dictionary<string, int> MaxPolyphony { get; set; } = [];

    public void Play(string name, bool processAlways = false, bool voice = false)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        if (GetNodeOrNull(name) is AudioStreamPlayer se && IsInstanceValid(se))
        {
            se.Play();
            return;
        }

        if (Lib.GetPackedScene($"res://game/contents/se/{name}.ogg") is not AudioStream audio)
        {
            return;
        }

        AudioStreamPlayer audioStreamPlayer = new()
        {
            Name = name,
            Bus = voice ? "VOICE" : "SE",
            MaxPolyphony = MaxPolyphony.TryGetValue(name, out int value) ? value : 1,
            Stream = audio,
            ProcessMode = processAlways ? ProcessModeEnum.Inherit : ProcessModeEnum.Pausable
        };

        AddChild(audioStreamPlayer);
        audioStreamPlayer.Play();
    }

    public void ClearAllAudioStreamPlayer()
    {
        foreach (Node n in GetChildren())
        {
            if (n is AudioStreamPlayer aplayer && !aplayer.Playing)
            {
                aplayer.Name = "remove";
                aplayer.QueueFree();
            }
        }
    }
}
