using Godot;

namespace teos.common.command;

/// <summary>
/// コマンドノードの親
/// </summary>
public partial class CommandRoot : Node, ICommand
{
    /// <summary>
    /// 実行フラグ
    /// </summary>
    [Export]
    public bool ExecFlag { get; set; } = true;

    public static void ExecChildren(Node root, Node node, bool flag)
    {
        if (root is null || !IsInstanceValid(root))
        {
            return;
        }

        if (root is ICommand command)
        {
            command.ExecCommand(node, flag);
        }
        else
        {
            ExecAllCommand(root, node, flag);
        }
    }

    public static void ExecAllCommand(Node root, Node node, bool flag)
    {
        if (root is null || !IsInstanceValid(root))
        {
            return;
        }

        foreach (Node child in root.GetChildren())
        {
            if (child is ICommand cnode)
            {
                cnode.ExecCommand(node, flag);
            }
        }
    }

    #region ICommandインタフェース
    public virtual void ExecCommand(Node node, bool flag)
    {
    }
    #endregion
}
