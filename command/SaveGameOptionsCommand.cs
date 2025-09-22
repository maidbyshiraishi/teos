using Godot;
using teos.system;

namespace teos.command;

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
