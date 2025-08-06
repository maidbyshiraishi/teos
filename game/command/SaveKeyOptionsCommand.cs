using Godot;
using teos.common.command;
using teos.game.system;

namespace teos.game.command;

/// <summary>
/// 操作設定を保存するコマンド
/// </summary>
public partial class SaveKeyOptionsCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<GameKeyOption>("/root/GameKeyOption").SaveKeyOptions();
    }
}
