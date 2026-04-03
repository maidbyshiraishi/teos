using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.option;

/// <summary>
/// ゲームオプションを保存するコマンド
/// </summary>
public partial class SaveGameOptionsCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<GameOption>("/root/GameOption").SaveOptions();
    }
}
