using Godot;
using teos.common.command;
using teos.game.system;

namespace teos.game.command;

/// <summary>
/// ステージ情報を保存するコマンド
/// </summary>
public partial class StateBackupCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<GameDataManager>("/root/GameDataManager").Backup();
    }
}
