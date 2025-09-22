using Godot;

namespace teos.command;

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
