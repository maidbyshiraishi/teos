using Godot;
using teos.system;

namespace teos.command.option;

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
