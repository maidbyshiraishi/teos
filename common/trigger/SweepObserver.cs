using Godot;
using Godot.Collections;

namespace teos.common.trigger;

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

    public override void _Process(double delta)
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
