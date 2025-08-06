using Godot;

namespace teos.common.command;

/// <summary>
/// 他コマンドコンテナを呼び出すコマンド
/// </summary>
public partial class ReferenceCommandContainer : CommandRoot
{
    /// <summary>
    /// 呼び出すコマンドコンテナ
    /// </summary>
    [Export]
    public Node Target { get; set; }

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag || Target is null)
        {
            return;
        }

        ExecAllCommand(Target, node, flag);
    }
}
