using Godot;
using teos.common.screen;

namespace teos.game.screen;

/// <summary>
/// 結果発表画面
/// </summary>
public partial class ResultScreen : DialogRoot
{
    private int _rank = -1;
    private int _score = 0;

    public override void _Ready()
    {
        base._Ready();
        HighScoreListControl highScoreListControl = GetNode<HighScoreListControl>("HighScoreListControl");
        highScoreListControl.HighlightRank(_rank);

        if (GetNodeOrNull("Score") is Label label)
        {
            label.Text = _score.ToString("N0");
        }
    }

    public override void GetArgument()
    {
        GetDialogArgument("ResultScreen");

        if (m_Argument is null || m_Argument.Count != 2 || m_Argument[0].VariantType is not Variant.Type.Int || m_Argument[1].VariantType is not Variant.Type.Int)
        {
            return;
        }

        _rank = (int)m_Argument[0];
        _score = (int)m_Argument[1];
    }
}
