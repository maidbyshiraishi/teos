using Godot;

namespace teos.common.command;

/// <summary>
/// 回数制限コンテナ
/// </summary>
public partial class LimitedTimesContainer : CommandRoot
{
    /// <summary>
    /// 制限回数
    /// </summary>
    [Export]
    public int NumberOfTimes { get; set; } = 1;

    private int _count = 0;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || NumberOfTimes == 0 || NumberOfTimes <= _count)
        {
            return;
        }

        _count++;
        ExecAllCommand(this, node, flag);
    }
}
