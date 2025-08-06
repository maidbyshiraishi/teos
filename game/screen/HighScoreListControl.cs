using Godot;
using System;
using System.Collections.Generic;
using teos.game.data;
using teos.game.system;

namespace teos.game.screen;

/// <summary>
/// ハイスコア一覧コントロール
/// </summary>
public partial class HighScoreListControl : Control
{
    public override void _Ready()
    {
        List<ScoreData> highScoreList = GetNode<HighScoreManager>("/root/HighScoreManager").GetHighScoreList();
        int length = Mathf.Min(highScoreList.Count, 10);

        for (int i = 0; i < length; i++)
        {
            Control rankControl = GetNode<Control>($"Rank_{i + 1}");
            rankControl.GetNode<Label>("Score").Text = highScoreList[i].Score.ToString("N0");
            rankControl.GetNode<Label>("Date").Text = DateTime.Parse(Time.GetDatetimeStringFromUnixTime(highScoreList[i].Date)).ToLocalTime().ToString();

            if (i > 0 && highScoreList[i].Score == highScoreList[i - 1].Score)
            {
                rankControl.GetNode<Label>("Rank").Text = "";
            }
        }
    }

    public void HighlightRank(int rank)
    {
        if (rank is < 0 or > 9)
        {
            return;
        }

        GetNode<RichTextLabel>("Rank_1/RichScore").Hide();
        Control rankControl = GetNode<Control>($"Rank_{rank + 1}");
        Label score = rankControl.GetNode<Label>("Score");
        RichTextLabel richScore = rankControl.GetNode<RichTextLabel>("RichScore");
        richScore.Text = $"[rainbow]{score.Text}[/rainbow]";
        richScore.Show();
    }
}
