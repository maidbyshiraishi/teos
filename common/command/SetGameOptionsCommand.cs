using Godot;
using teos.common.system;

namespace teos.common.command;

/// <summary>
/// ゲームオプションをセットするコマンド
/// </summary>
public partial class SetGameOptionsCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<GameOption>("/root/GameOption").SetOptions();
    }
}
