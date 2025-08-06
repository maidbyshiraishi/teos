using Godot;
using teos.common.command;
using teos.game.system;

namespace teos.game.command;

/// <summary>
/// 操作設定をデフォルト値にリセットするコマンド
/// </summary>
public partial class ResetDefaultKeyOptionsCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<GameKeyOption>("/root/GameKeyOption").ResetDefaultKeyOptions();
    }
}
