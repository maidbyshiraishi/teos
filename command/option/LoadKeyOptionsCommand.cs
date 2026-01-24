using Godot;
using teos.system;

namespace teos.command.option;

/// <summary>
/// 操作設定をロードするコマンド
/// </summary>
public partial class LoadKeyOptionsCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<GameKeyOption>("/root/GameKeyOption").LoadKeyOptions();
    }
}
