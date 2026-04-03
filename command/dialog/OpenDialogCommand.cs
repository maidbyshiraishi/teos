using Godot;
using Godot.Collections;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.dialog;

/// <summary>
/// ダイアログを開くコマンド
/// </summary>
public partial class OpenDialogCommand : CommandRoot
{
    /// <summary>
    /// 開くダイアログ
    /// </summary>
    [Export]
    public string DialogPath { get; set; }

    [Export]
    public string ArgumentKey { get; set; }

    [Export]
    public Array<Variant> Argument { get; set; } = [];

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").OpenDialog(DialogPath, ArgumentKey, Argument);
    }
}
