using Godot;
using Godot.Collections;

namespace teos.common.command;

/// <summary>
/// CallGroupを実行する
/// </summary>
public partial class CallGroupCommand : CommandRoot
{
    /// <summary>
    /// フラグ名
    /// </summary>

    [Export]
    public string CallGroupName { get; set; }

    /// <summary>
    /// フラグ値
    /// </summary>
    [Export]
    public string CallMethodName { get; set; }

    [Export]
    public Array<Variant> CallArgs { get; set; } = [];

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        Variant[] variant = new Variant[CallArgs.Count];
        CallArgs.CopyTo(variant, 0);
        GetTree().CallGroup(CallGroupName, CallMethodName, variant);
    }
}
