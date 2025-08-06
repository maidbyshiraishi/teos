using Godot;
using System;
using teos.common.data;

namespace teos.game.data;

/// <summary>
/// スコアデータのセット
/// </summary>
public class ScoreData : DataRoot, IComparable<ScoreData>
{
    public int Score { get; set; } = -1;
    public long Date { get; set; } = -1;

    public ScoreData()
    {
    }

    public ScoreData(int score, long date)
    {
        Score = score;
        Date = date;
    }

    public int CompareTo(ScoreData other)
    {
        if (other is null)
        {
            return -1;
        }

        // スコアが同じ場合
        if (Score == other.Score)
        {
            // 日付の昇順
            return Mathf.Sign(Date - other.Date);
        }

        // スコアの逆順
        return other.Score - Score;
    }
}
