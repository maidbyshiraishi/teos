using Godot;
using teos.common.system;

namespace teos.common.command;

/// <summary>
/// ステージ情報を保存するコマンド
/// </summary>
public partial class StateSaveCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<DialogLayer>("/root/DialogLayer").GetCurrentStageRoot().SaveState();
    }
}
