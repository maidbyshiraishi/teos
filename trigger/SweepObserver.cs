using Godot;
using Godot.Collections;

namespace maid_by_shiraishi.trigger;

/// <summary>
/// グループ内のノード数がゼロになった場合にコマンドを実行するトリガー
/// </summary>
public partial class SweepObserver : ProcessTriggerRoot
{
    /// <summary>
    /// 監視するグループ
    /// </summary>
    [Export]
    public string GroupName { get; set; }

    private bool _opened = false;

    public override void _PhysicsProcess(double delta)
    {
        if (_opened || string.IsNullOrWhiteSpace(GroupName))
        {
            return;
        }

        Array<Node> group = GetTree().GetNodesInGroup(GroupName);

        if (group.Count == 0)
        {
            _opened = true;
            Exec();
        }
    }
}
