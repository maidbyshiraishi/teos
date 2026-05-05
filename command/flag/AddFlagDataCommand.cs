using Godot;
using maid_by_shiraishi.system;

namespace maid_by_shiraishi.command.flag;

/// <summary>
/// ゲームフラグを操作するコマンド
/// </summary>
public partial class AddFlagDataCommand : CommandRoot
{
    /// <summary>
    /// フラグ名
    /// </summary>
    [Export]
    public string Key { get; set; }

    /// <summary>
    /// フラグ値
    /// </summary>
    [Export]
    public int Value { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<GameDataManager>("/root/GameDataManager").AddFlagData(Key, Value);
    }
}
