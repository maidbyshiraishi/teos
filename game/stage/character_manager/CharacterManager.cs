using Godot;
using Godot.Collections;
using teos.game.system;

namespace teos.game.stage.character_manager;

/// <summary>
/// キャラクター有効化・無効化制御
/// 『宇宙から来たメイド』後に実装された、最も重要な処理の一つ
/// 
/// CharacterGroupに属しているノードに対して活性化と非活性化の制御をおこなう。
///
/// ・すべての処理はGameStageRootの_Ready()からスタートする。
///
/// 初期化処理
/// ・CharacterGroupに属しているノードを列挙し、それらノードに下記の処理を行う。
///     -対象ノードがICharacterManagerではない場合、CharacterManagerの制御対象でないため、処理を中断する
///     -対象ノードがICharacterManagerの場合かつ対象ノードがNode2Dの場合、非表示にする。
///     -対象ノード内にICharacterManagerEnabler系ノードが存在しない場合、活性化処理を行う。
///     -対象ノード内にICharacterManagerEnabler系ノードが存在する場合、
///          *対象ノード内のICharacterManagerEnabler系ノードをCharacterManagerノードに移動する。
///              >ICharacterManagerEnablerインタフェースのReparentCharacterEnabler()の実装によってはCharacterManagerのノード以外への移動も可能
///          *対象ノードのProcessModeをDisabledに変更する。
///          
/// 活性化処理
/// ・ゲーム中にCharacterEnabler系ノードに設定された条件を満たした場合、CharacterManagerの活性化処理を実行する。
/// ・CharacterManagerは対象ノードを表示し、対象ノードのProcessModeをInheritに変更し、ActiveCharacter(true)を実行することで対象ノードが活性化する。
///
/// 非活性化処理
/// ・CharacterManagerの非活性化処理を実行する
/// ・CharacterManagerは対象ノードを非表示にし、対象ノードのProcessModeをDisabledに変更し、ActiveCharacter(false)を実行することで対象ノードが非活性化する。
/// </summary>
public partial class CharacterManager : Node2D
{
    public static readonly string CharacterGroup = "CharacterGroup";

    public void EntryCharacterNodes(Node node, Node cnode)
    {
        if (node is not ICharacterManager inode)
        {
            return;
        }

        if (node is Node2D node2d)
        {
            node2d.Visible = false;
        }

        if (cnode is not ICharacterManagerEnabler cenabler)
        {
            EnableCharacterNode(inode, true);
            return;
        }

        // 各種CharacterEnablerがセットされている場合は登録を行う
        cenabler.SetCharacterManager(this);
        _ = GetNode<GameDialogLayer>("/root/DialogLayer").GetCurrentGameRoot();

        if (cnode.GetParent() is null)
        {
            AddChild(cnode);
        }
        else
        {
            cenabler.ReparentCharacterEnabler(this);
        }

        node.ProcessMode = ProcessModeEnum.Disabled;
    }

    public void EntryCharacterNodes()
    {
        Array<Node> group = GetTree().GetNodesInGroup(CharacterGroup);

        foreach (Node node in group)
        {
            Node cnode = FindCharacterEnabler(node);
            EntryCharacterNodes(node, cnode);
        }
    }

    private static Node FindCharacterEnabler(Node root)
    {
        if (root is null)
        {
            return null;
        }

        foreach (Node n in root.GetChildren())
        {
            if (n is ICharacterManagerEnabler)
            {
                return n;
            }
        }

        return null;
    }

    public void EnableCharacterNode(ICharacterManager inode, bool enable)
    {
        if (inode is null)
        {
            return;
        }

        if (inode is Node2D node2d)
        {
            node2d.Visible = enable;
        }

        if (inode is Node node)
        {
            node.ProcessMode = enable ? ProcessModeEnum.Inherit : ProcessModeEnum.Disabled;
        }

        inode.SetCharacterManager(this);
        inode.ActiveCharacter(enable);
    }
}
