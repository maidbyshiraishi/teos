using Godot;

namespace teos.common.command;

/// <summary>
/// デバッグログ
/// </summary>
public partial class PrintCommand : CommandRoot
{
    /// <summary>
    /// ログ
    /// </summary>
    [Export]
    public string Log { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || string.IsNullOrWhiteSpace(Log))
        {
            return;
        }

        GD.Print(Log);
    }
}
