using Godot;
using teos.system;

namespace teos.command;

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
