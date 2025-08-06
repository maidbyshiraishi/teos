using Godot;
using teos.common.theater;

namespace teos.common.command;

/// <summary>
/// ストーリーシアターの指定ページへ移動するコマンド
/// </summary>
public partial class GoPageCommand : CommandRoot, IStoryTheaterContent
{
    /// <summary>
    /// 移動先のページ
    /// </summary>
    [Export]
    public Control Page { get; set; }

    private StoryTheater _storyTheater = null;

    public override void _Ready()
    {
        AddToGroup(StoryTheater.StoryTheaterContent);
    }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        _storyTheater?.GoPage(Page);
    }

    #region IStoryTheaterContentインタフェース
    public void InitializeStoryTheaterContent(StoryTheater storyTheater)
    {
        _storyTheater = storyTheater;
    }
    #endregion
}
