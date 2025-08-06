using Godot;
using teos.common.system;

namespace teos.common.command;

/// <summary>
/// 指定スクリーンへ移動するコマンド
/// </summary>
public partial class OpenScreenCommand : CommandRoot
{
    /// <summary>
    /// 次スクリーンのリソースパス
    /// </summary>
    [Export]
    public string Screen { get; set; }

    /// <summary>
    /// フェードアウトエフェクト
    /// </summary>
    [Export]
    public string Fadeout { get; set; } = "fadeout_1";

    /// <summary>
    /// フェードインエフェクト
    /// </summary>
    [Export]
    public string Fadein { get; set; } = "fadein_1";

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").OpenScreen(Screen, Fadeout, Fadein);
    }
}
