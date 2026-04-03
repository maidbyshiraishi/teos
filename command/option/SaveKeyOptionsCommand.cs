using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.option;

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
