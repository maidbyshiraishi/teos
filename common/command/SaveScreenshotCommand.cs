using Godot;
using System;

namespace teos.common.command;

/// <summary>
/// スクリーンショット保存コマンド
/// </summary>
public partial class SaveScreenshotCommand : CommandRoot
{
    /// <summary>
    /// スクリーンショットのパス
    /// </summary>
    [Export]
    public string ScreenshotPath { get; set; } = "user://teos_{0}.png";

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        Image image = GetViewport().GetTexture().GetImage();
        DateTime datetime = DateTime.Now;
        string datetimeString = datetime.ToString("yyyyMMddHHmmss");
        string file = string.Format(ScreenshotPath, datetimeString);
        Error e = image.SavePng(file);

        if (e is not Error.Ok)
        {
            GD.PrintErr($"スクリーンショットの保存中にエラーが発生しました。ゲームは続行されます。エラーの値は{e}です。");
        }
    }
}
