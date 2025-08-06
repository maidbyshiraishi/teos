using Godot;
using teos.common.command;

namespace teos.game.command;

/// <summary>
/// 敵をすべて倒すコマンド
/// </summary>
public partial class SweepCommand : CommandRoot
{
    /// <summary>
    /// すべて倒すグループ名
    /// </summary>
    [Export]
    public string SweepTargetGroup { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetTree().CallGroup(SweepTargetGroup, "Sweep");
    }
}
