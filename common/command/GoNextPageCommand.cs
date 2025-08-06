using Godot;
using teos.common.theater;

namespace teos.common.command;

/// <summary>
/// ストーリーシアターの次ページへ移動するコマンド
/// </summary>
public partial class GoNextPageCommand : CommandRoot, IStoryTheaterContent
{
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

        _storyTheater?.GoNextPage();
    }

    #region IStoryTheaterContentインタフェース
    public void InitializeStoryTheaterContent(StoryTheater storyTheater)
    {
        _storyTheater = storyTheater;
    }
    #endregion
}
