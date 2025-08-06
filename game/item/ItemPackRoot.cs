using Godot;
using Godot.Collections;
using teos.game.stage.character_manager;
using teos.game.system;

namespace teos.game.item;

/// <summary>
/// アイテムをセットで表示・非表示させる入れ物
/// </summary>
public partial class ItemPackRoot : Node2D, ICharacterManager
{
    private CharacterManager _characterManager;

    public override void _Ready()
    {
        AddToGroup(CharacterManager.CharacterGroup);
    }

    #region ICharacterManagerインタフェース
    public void ActiveCharacter(bool active)
    {
        if (active)
        {
            GetNode<GameDialogLayer>("/root/DialogLayer").GetCurrentGameRoot().ReparentNode(this, "Item");
            DelayActiveItems();
        }
    }

    private async void DelayActiveItems()
    {
        // アイテムのアクティブ化は延滞実行する。
        // アイテムのVisibleOnScreenNotifier2Dが非表示を返してアイテムが消滅する。
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        _ = await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        Array<Node> children = GetChildren();
        int count = children.Count;
        float angle = Mathf.DegToRad(360f / count);

        for (int i = 0; i < count; i++)
        {
            if (children[i] is ItemRoot item)
            {
                item.SetDirection(angle * i);
            }
        }
    }

    public void SetCharacterManager(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }
    #endregion
}
