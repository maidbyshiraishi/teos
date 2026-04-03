using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.state;

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

        GetNode<DialogLayer>("/root/DialogLayer").GetCurrentGameStageRoot().SaveState();
    }
}
