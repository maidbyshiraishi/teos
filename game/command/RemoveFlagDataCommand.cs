using Godot;
using teos.common.command;
using teos.game.system;

namespace teos.game.command;

/// <summary>
/// ゲームフラグを削除するコマンド
/// </summary>
public partial class RemoveFlagDataCommand : CommandRoot
{
    /// <summary>
    /// フラグ名
    /// </summary>
    [Export]
    public string Target { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<GameDataManager>("/root/GameDataManager").RemoveFlagData(Target);
    }
}
