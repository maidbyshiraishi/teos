using Godot;

namespace teos.common.command;

/// <summary>
/// Nodeを開放するコマンド
/// </summary>
public partial class QueueFreeCommand : CommandRoot
{
    /// <summary>
    /// 開放するノード
    /// </summary>
    [Export]
    public Node Target { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || Target is null)
        {
            return;
        }

        _ = Target.CallDeferred(Node.MethodName.QueueFree);
    }
}
