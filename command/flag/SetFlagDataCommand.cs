using Godot;
using teos.system;

namespace teos.command.flag;

/// <summary>
/// フラグデータをセットするコマンド
/// </summary>
public partial class SetFlagDataCommand : CommandRoot
{
    /// <summary>
    /// フラグ名
    /// </summary>
    [Export]
    public string Target { get; set; }

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

        GetNode<GameDataManager>("/root/GameDataManager").SetFlagData(Target, Value);
    }
}
