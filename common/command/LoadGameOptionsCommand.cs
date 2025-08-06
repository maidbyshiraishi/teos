using Godot;
using teos.common.system;

namespace teos.common.command;

/// <summary>
/// ゲーム設定をロードするコマンド
/// </summary>
public partial class LoadGameOptionsCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<GameOption>("/root/GameOption").LoadOptions();
    }
}
