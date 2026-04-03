using Godot;

namespace maid_by_shiraishi.command;

/// <summary>
/// コマンドコンテナ
/// </summary>
public partial class CommandContainer : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        ExecAllCommand(this, node, flag);
    }
}
