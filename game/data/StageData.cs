using Godot;
using Godot.Collections;
using teos.common.data;

namespace teos.game.data;

/// <summary>
/// ステージデータ
/// </summary>
public class StageData : DataRoot
{
    public static readonly string SectionName = "StageData";
    public static readonly string StageNoKey = "StageNo";
    public static readonly Array<string> NecessaryKey = [StageNoKey];
    public static readonly Array<string> AllKey = [StageNoKey];

    public int StageNo { get; set; }

    public StageData(int stageNo = 1)
    {
        StageNo = stageNo;
    }

    public StageData Copy()
    {
        return new()
        {
            StageNo = StageNo,
        };
    }

    public override Error GetConfigFile(ConfigFile file)
    {
        StageNo = GetData(file, SectionName, StageNoKey);
        return Error.Ok;
    }

    public override Error SetConfigFile(ConfigFile file)
    {
        SetData(file, SectionName, StageNoKey, StageNo);
        return Error.Ok;
    }

    public override Error CheckNecessaryKey(ConfigFile file)
    {
        foreach (string key in NecessaryKey)
        {
            if (!HasData(file, SectionName, key))
            {
                GD.PrintErr($"StageDataに必須キー{key}が存在しません。");
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
                GD.PrintErr($"StageDataには存在しないキー{key}を削除します。");
                RemoveData(file, SectionName, key);
            }
        }
    }

    public override string[] GetSectionKeys(ConfigFile file)
    {
        return [.. NecessaryKey];
    }

    public override Array GetSectionValues(ConfigFile file)
    {
        return [StageNo];
    }
}
