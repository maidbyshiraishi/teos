using Godot;
using Godot.Collections;
using teos.common.screen;
using teos.common.stage;

namespace teos.common.system;

/// <summary>
/// 画面遷移・ダイアログ制御
/// ゲーム全体に影響する最も重要な処理の一つ
/// 
/// スクリーンを開く場合、スタック上のダイアログをすべて閉じ、指定されたスクリーンをメインシーンとして読み込む。
/// ダイアログを開く場合、スタックに新たにダイアログを積む。
/// 
/// 新たにダイアログやスクリーンを開くと、一番前面のダイアログやスクリーンのControlノード内のボタンなどのGUIが無効になり、プロセスが休止される。
/// ダイアログを閉じると、閉じたダイアログのスタックに積まれた次のダイアログのControlノード内あるいはダイアログが開いていない場合はスクリーンのControlノード内のボタンなどのGUIが有効になり、プロセスが再開される。
///
/// ゲームの終了処理も含む。
/// </summary>
public partial class DialogLayer : CanvasLayer
{
    private readonly Array<DialogRoot> _history = [];

    /// <summary>
    /// 現在の画面を返す
    /// </summary>
    /// <returns>ScreenRoot</returns>
    public DialogRoot GetCurrentScreen()
    {
        return GetTree().CurrentScene as DialogRoot;
    }

    /// <summary>
    /// 現在のゲーム画面を返す
    /// </summary>
    /// <returns>ScreenRoot</returns>
    public StageRoot GetCurrentStageRoot()
    {
        return GetTree().CurrentScene as StageRoot;
    }

    /// <summary>
    /// 現在のダイアログを返す
    /// </summary>
    /// <returns>DialogRoot</returns>
    public DialogRoot GetCurrentDialog()
    {
        return Pop(false);
    }

    /// <summary>
    /// ダイアログを開く
    /// </summary>
    /// <param name="path">ダイアログパス</param>
    /// <param name="key">引数キー</param>
    /// <param name="argument">引数配列</param>
    public void OpenDialog(string path, string key = null, Array<Variant> argument = null)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PrintErr("pathがnullまたはホワイトスペースです。OpenDialog()を実行できません。");
            return;
        }

        if (!string.IsNullOrWhiteSpace(key))
        {
            GetNode<DialogArgument>("/root/DialogArgument").SetArgument(key, argument);
        }

        GetTree().Paused = true;
        _ = CallDeferred(MethodName.DeferredOpenDialog, [path]);
    }

    private void DeferredOpenDialog(string path)
    {
        if (Lib.GetPackedScene(path) is not PackedScene pack || pack.Instantiate() is not Node node)
        {
            return;
        }

        DialogRoot screen = GetCurrentScreen();

        if (screen is null)
        {
            GD.PrintErr($"スクリーンが開いていない状態でOpenDialog()を実行できません。");
            return;
        }

        if (node is not DialogRoot dnode)
        {
            GD.PrintErr($"{path}はダイアログではありません。OpenDialog()を実行できません。");
            return;
        }

        // ダイアログが開いていないなら画面を停止させる
        if (IsEmpty())
        {
            screen.Inactive();
            GetTree().Paused = true;
        }
        // ダイアログが開いているなら停止させる
        else
        {
            GetCurrentDialog().Inactive();
            GetCurrentDialog().ProcessMode = ProcessModeEnum.Pausable;
        }

        Push(dnode);
        AddChild(dnode);
        dnode.Active();
        Input.MouseMode = dnode.MouseCaptured ? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible;
    }

    /// <summary>
    /// 開いているダイアログをすべて閉じる
    /// </summary>
    /// <param name="undo">アンドゥを実施するか</param>
    public void CloseAllDialog(bool undo = false)
    {
        while (!IsEmpty())
        {
            CloseDialog(undo, true);
        }
    }

    /// <summary>
    /// ダイアログを閉じる
    /// </summary>
    /// <param name="undo">アンドゥを実施するか</param>
    /// <param name="skipActive">Controlをアクティブに戻さない</param>
    public void CloseDialog(bool undo = false, bool skipActive = false)
    {
        DialogRoot dnode = Pop(true);

        if (dnode is null)
        {
            return;
        }

        if (undo)
        {
            dnode.Undo();
        }

        dnode.Close();

        if (IsEmpty())
        {
            if (!skipActive)
            {
                DialogRoot current = GetCurrentScreen();
                current.Active();
                Input.MouseMode = current.MouseCaptured ? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible;
            }

            GetTree().Paused = false;
        }
        else if (!skipActive)
        {
            DialogRoot dialogRoot = GetCurrentDialog();

            if (dialogRoot is not null)
            {
                dialogRoot.Active();
                Input.MouseMode = dialogRoot.MouseCaptured ? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible;
                dialogRoot.ProcessMode = ProcessModeEnum.Inherit;
            }
        }
    }

    public void UpdateDialogScreen()
    {
        GetCurrentDialog()?.UpdateDialogScreen();
    }

    /// <summary>
    /// 画面を開く
    /// </summary>
    /// <param name="path">パス</param>
    /// <param name="fadein">画面のフェードインエフェクト名</param>
    /// <param name="fadeout">画面のフェードアウトエフェクト名</param>
    public void OpenScreen(string path, string fadeout, string fadein, string key = null, Array<Variant> argument = null)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PrintErr("pathがnullまたはホワイトスペースです。ChangeSceneToFile()できません。");
            return;
        }

        if (!string.IsNullOrWhiteSpace(key))
        {
            GetNode<DialogArgument>("/root/DialogArgument").SetArgument(key, argument);
        }

        GetTree().Paused = true;
        DialogRoot dialog = GetCurrentDialog();

        if (dialog is null)
        {
            DialogRoot screen = GetCurrentScreen();
            screen?.Inactive();
        }
        else
        {
            dialog.Inactive();
        }

        _ = CallDeferred(MethodName.DeferredOpenScreen, [path, fadeout, fadein]);
    }

    protected async void DeferredOpenScreen(string path, string fadeout, string fadein)
    {
        ScreenFader fader = GetNode<ScreenFader>("/root/ScreenFader");
        fader.ScreenFade(fadeout);
        _ = await ToSignal(fader, ScreenFader.SignalName.ScreenFadeFinished);
        CloseAllDialog();

        if (Lib.GetPackedScene(path) is PackedScene pack)
        {
            _ = GetTree().ChangeSceneToPacked(pack);
        }

        fader.ScreenFade(fadein);
        _ = await ToSignal(fader, ScreenFader.SignalName.ScreenFadeFinished);
        DialogRoot current = GetCurrentScreen();
        current.Active();
        Input.MouseMode = current.MouseCaptured ? Input.MouseModeEnum.Captured : Input.MouseModeEnum.Visible;
        GetTree().Paused = false;
    }

    private bool IsEmpty()
    {
        return _history.Count == 0;
    }

    private void Push(DialogRoot droot)
    {
        _history.Insert(0, droot);
    }

    private DialogRoot Pop(bool delete)
    {
        if (IsEmpty())
        {
            return null;
        }

        DialogRoot droot = _history[0];

        if (delete)
        {
            _history.RemoveAt(0);
        }

        return droot;
    }

    public void QuitGame()
    {
        GetTree().Quit();
    }
}
