using Godot;
using teos.common.system;

namespace teos.common.command;

/// <summary>
/// 効果音を再生するコマンド
/// </summary>
public partial class PlaySeCommand : CommandRoot
{
    /// <summary>
    /// 効果音名
    /// </summary>
    [Export]
    public string SeName { get; set; }

    /// <summary>
    /// プロセスモードをAlwaysにするか
    /// </summary>
    [Export]
    public bool ProcessAlways { get; set; } = false;

    [Export]
    public bool Voice { get; set; } = false;

    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        GetNode<SePlayer>("/root/SePlayer").Play(SeName, ProcessAlways, Voice);
    }
}
