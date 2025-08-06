using Godot;
using teos.common.system;

namespace teos.common.command;

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
