using Godot;
using teos.common.theater;

namespace teos.common.command;

/// <summary>
/// スクロールシアターのスクロールを切り替えるコマンド
/// </summary>
public partial class SetScrollCommand : CommandRoot, IScrollTheaterContent
{
    /// <summary>
    /// スクロールシアターのスクロール状態
    /// </summary>
    [Export]
    public bool Running { get; set; } = false;

    private ScrollTheater _scrollTheater = null;

    public override void _Ready()
    {
        AddToGroup(ScrollTheater.ScrollTheaterContent);
    }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || _scrollTheater is null)
        {
            return;
        }

        _scrollTheater.SetScroll(Running);
    }

    #region IScrollTheaterContentインタフェース
    public void InitializeScrollTheaterContent(ScrollTheater scrollTheater)
    {
        _scrollTheater = scrollTheater;
    }
    #endregion
}
