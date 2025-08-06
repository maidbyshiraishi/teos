using Godot;
using teos.common.system;

namespace teos.common.command;

/// <summary>
/// ゲーム設定をデフォルト値にリセットするコマンド
/// </summary>
public partial class ResetDefaultGameOptionsCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<GameOption>("/root/GameOption").ResetOptions();
    }
}
