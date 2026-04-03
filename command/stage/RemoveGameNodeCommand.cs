using Godot;
using maid_by_shiraishi.stage;

namespace maid_by_shiraishi.command.stage;

/// <summary>
/// 接触したGameNodeを除去するコマンド
/// </summary>
public partial class RemoveGameNodeCommand : CommandRoot
{
    /// <summary>
    /// 開放するノード
    /// </summary>
    [Export]
    public Node Target { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || (Target is null ? node : Target) is not IGameNode inode)
        {
            return;
        }

        inode.RemoveNode();
    }
}
