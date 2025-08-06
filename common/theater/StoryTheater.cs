using Godot;
using teos.common.command;
using teos.common.screen;

namespace teos.common.theater;

/// <summary>
/// ストーリーシアターダイアログ
/// </summary>
public partial class StoryTheater : DialogRoot
{
    public static readonly string StoryTheaterContent = "StoryTheaterContent";

    // 文字を修飾したい場合はBBCode機能を有効にする
    // https://docs.godotengine.org/ja/4.x/tutorials/ui/bbcode_in_richtextlabel.html
    // [center]中央揃え[/center]
    // [color=red]文字色[/color]
    // [wave]ウェーブ[/wave]
    // [tornado]トルネード[/tornado]
    // [shake]シェイク[/shake]
    // [fade start=0 length=9]フェード[/fade]
    // [rainbow]ゲーミング[/rainbow]
    // [outline_size=5]縁取り[/outline_size]
    // [outline_color=red]縁取り色[/outline_color]

    private Control _content;
    private Control _socket;
    private int _index = 0;

    public override void _Ready()
    {
        _socket = GetNode<Control>("Socket");
        _content = _socket.GetNode<Control>("Contents");
        GetTree().CallGroup(StoryTheaterContent, "InitializeStoryTheaterContent", [this]);
    }

    public override void InitializeNode()
    {
        CloseAllPages();
        OpenPage(_index);
    }

    /// <summary>
    /// すべてのページを閉じる
    /// </summary>
    private void CloseAllPages()
    {
        if (_content is null)
        {
            return;
        }

        int length = _content.GetChildren().Count;

        for (int i = 0; i < length; i++)
        {
            ClosePage(i);
        }
    }

    /// <summary>
    /// 現在のページを閉じる
    /// </summary>
    private void ClosePage(int index)
    {
        if (_content is null)
        {
            return;
        }

        Control nowNode = _content.GetChildOrNull<Control>(index);

        if (nowNode is not null)
        {
            ChangeFocusMode(false, nowNode);
            nowNode.Hide();
        }
    }

    private void OpenPage(int index)
    {
        if (_content is null)
        {
            return;
        }

        Control nextNode = _content.GetChildOrNull<Control>(index);

        if (nextNode is not null)
        {
            ChangeFocusMode(true, nextNode);
            nextNode.Show();
            SetFocusFirst(nextNode);
            _index = index;
        }
    }

    private void OpenPage(Control control)
    {
        if (_content is null)
        {
            return;
        }

        int index = 0;

        foreach (Node n in _content.GetChildren())
        {
            if (n is Control c && c == control)
            {
                OpenPage(index);
                return;
            }

            index++;
        }
    }

    public void GoPage(Control control)
    {
        ClosePage(_index);
        OpenPage(control);
    }

    /// <summary>
    /// 次のページを表示する
    /// </summary>
    public void GoNextPage()
    {
        ClosePage(_index);
        _index++;
        OpenPage(_index);
    }

    public override void GetArgument()
    {
        GetDialogArgument("StoryTheater");
    }

    private void CloseStory()
    {
        CommandRoot.ExecChildren(GetNodeOrNull("CloseCommand"), this, true);
    }
}
