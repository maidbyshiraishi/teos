using Godot;
using Godot.Collections;
using teos.common.data;

namespace teos.game.data;

/// <summary>
/// プレーヤーデータ
/// </summary>
public class PlayerData : DataRoot
{
    public static readonly string SectionName = "PlayerData";
    public static readonly string RemainKey = "Remain";
    public static readonly string LifeKey = "Life";
    public static readonly string ScoreKey = "Score";
    public static readonly string ExtendScoreKey = "ExtendScore";
    public static readonly Array<string> NecessaryKey = [RemainKey, LifeKey, ScoreKey, ExtendScoreKey];
    public static readonly Array<string> AllKey = [RemainKey, LifeKey, ScoreKey, ExtendScoreKey];
    public static readonly int InitialRemain = 3;
    public static readonly int ExtendScoreThreshold = 70000;
    public static readonly int ExtendOverflowBonus = 30000;
    public static readonly int MaxRemain = 10;

    public int Remain { get; set; }
    public int Score { get; set; }
    public int ExtendScore { get; set; }

    private readonly Mutex _mutex = new();

    public PlayerData()
    {
        Remain = InitialRemain;
        Score = 0;
        ExtendScore = 0;
    }

    public PlayerData Copy()
    {
        return new()
        {
            Remain = Remain,
            Score = Score,
            ExtendScore = ExtendScore,
        };
    }

    public override Error SetConfigFile(ConfigFile file)
    {
        SetData(file, SectionName, RemainKey, Remain);
        SetData(file, SectionName, ScoreKey, Score);
        SetData(file, SectionName, ExtendScoreKey, ExtendScore);
        return Error.Ok;
    }

    public override Error GetConfigFile(ConfigFile file)
    {
        Remain = GetData(file, SectionName, RemainKey);
        Score = GetData(file, SectionName, ScoreKey);
        ExtendScore = GetData(file, SectionName, ExtendScoreKey);
        return Error.Ok;
    }

    public override Error CheckNecessaryKey(ConfigFile file)
    {
        foreach (string key in NecessaryKey)
        {
            if (!HasData(file, SectionName, key))
            {
                GD.PrintErr($"PlayerDataに必須キー{key}が存在しません。");
                return Error.InvalidData;
            }
        }

        return Error.Ok;
    }

    public override void RemoveIllegalKey(ConfigFile file)
    {
        string[] keys = file.GetSectionKeys(SectionName);

        foreach (string key in keys)
        {
            if (!AllKey.Contains(key))
            {
                GD.PrintErr($"PlayerDataには存在しないキー{key}を削除します。");
                RemoveData(file, SectionName, key);
            }
        }
    }

    public override string[] GetSectionKeys(ConfigFile file)
    {
        return [.. AllKey];
    }

    public override Array GetSectionValues(ConfigFile file)
    {
        return [Remain, Score, ExtendScore];
    }

    public int AddScore(int score)
    {
        _mutex.Lock();
        Score += score;
        ExtendScore += score;
        int extend = ExtendScore / ExtendScoreThreshold;
        ExtendScore %= ExtendScoreThreshold;
        AddRemain(extend);
        _mutex.Unlock();
        return extend;
    }

    public void AddRemain(int value)
    {
        int oldRemain = Remain;
        Remain = Mathf.Clamp(Remain + value, 0, MaxRemain);

        // 残機の上限突破ボーナス
        if (Remain == MaxRemain && oldRemain == MaxRemain)
        {
            Score += ExtendOverflowBonus;
        }
    }
}
