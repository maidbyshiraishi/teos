using Godot;
using teos.mob;

namespace teos.command;

/// <summary>
/// ライフを操作するコマンド
/// </summary>
public partial class AddLifeCommand : CommandRoot
{
    /// <summary>
    /// ライフの増減
    /// </summary>
    [Export]
    public int Value { get; set; } = 1;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || node is not ILife inode)
        {
            return;
        }

        inode.AddLife(Value);
    }
}
