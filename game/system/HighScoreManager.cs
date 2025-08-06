using Godot;
using System.Collections.Generic;
using teos.game.data;

namespace teos.game.system;

/// <summary>
/// ハイスコア管理
/// プロジェクト設定＞グローバル＞自動読み込みで自動的に実行が開始される。
/// </summary>
public partial class HighScoreManager : Node
{
    /// <summary>
    /// 保存時に暗号化
    /// </summary>
    [Export]
    public bool UseEncriptData { get; set; } = false;

    [Export]
    public string Password { get; set; } = "password";

    private static readonly string DataFilePath = "user://high_score.dat";
    private List<ScoreData> _highScoreList = null;
    private int _lastEntry = -1;

    public override void _Ready()
    {
        _highScoreList = LoadHighScore();
    }

    private List<ScoreData> LoadHighScore()
    {
        ConfigFile highScoreFile = new();
        Error e = UseEncriptData ? highScoreFile.LoadEncryptedPass(DataFilePath, Password) : highScoreFile.Load(DataFilePath);

        if (e is Error.FileNotFound)
        {
            return [];
        }
        else if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{DataFilePath}を読み込めませんでした。エラーコードは{e}です。ゲームのデフォルト値を使用します。");
            return [];
        }

        List<ScoreData> highScoreList = [];

        for (int i = 0; i < 10; i++)
        {
            if (!highScoreFile.HasSection($"Rank{i + 1}") || !highScoreFile.HasSectionKey($"Rank{i + 1}", "Score") || !highScoreFile.HasSectionKey($"Rank{i + 1}", "Date"))
            {
                break;
            }

            int score = highScoreFile.GetValue($"Rank{i + 1}", "Score", -1).AsInt32();
            long date = highScoreFile.GetValue($"Rank{i + 1}", "Date", -1).AsInt64();

            if (score == -1 || date == -1)
            {
                break;
            }

            highScoreList.Add(new(score, date));
        }

        highScoreList.Sort();
        return highScoreList;
    }

    public void SaveHighScore()
    {
        _highScoreList ??= [];

        int length = Mathf.Min(_highScoreList.Count, 10);
        ConfigFile highScoreFile = new();

        for (int i = 0; i < length; i++)
        {
            highScoreFile.SetValue($"Rank{i + 1}", "Score", _highScoreList[i].Score);
            highScoreFile.SetValue($"Rank{i + 1}", "Date", _highScoreList[i].Date);
        }

        Error e = UseEncriptData ? highScoreFile.SaveEncryptedPass(DataFilePath, Password) : highScoreFile.Save(DataFilePath);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"設定ファイル{DataFilePath}を保存できませんでした。エラーコードは{e}です。");
        }
    }

    public int EntryHighScore(int score)
    {
        if (score == 0)
        {
            return -1;
        }

        long date = Time.GetUnixTimeFromDatetimeDict(Time.GetDatetimeDictFromSystem(true));
        ScoreData scoreData = new(score, date);
        _highScoreList.Add(scoreData);
        _highScoreList.Sort();

        if (_highScoreList.Count > 10)
        {
            _highScoreList = _highScoreList[..10];
        }

        SaveHighScore();
        _lastEntry = _highScoreList.IndexOf(scoreData);
        return _lastEntry;
    }

    public List<ScoreData> GetHighScoreList()
    {
        return _highScoreList;
    }
}
