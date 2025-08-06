using Godot;
using teos.common.system;
using teos.game.stage.character_manager;
using teos.game.system;

namespace teos.game.decoration;

/// <summary>
/// 飾りエフェクト
/// </summary>
public partial class Decoration : Node2D, ICharacterManager
{
    public static readonly string ParentNodeName = "Decoration_1";
    private static readonly string StaticNodeName = "AutoScrollPath/AutoScroll/StaticDecoration";

    [Export]
    public string SeName { get; set; }

    [Export]
    public bool StaticOnScreen { get; set; } = false;

    public override void _Ready()
    {
        AddToGroup(CharacterManager.CharacterGroup);
    }

    #region ICharacterManagerインタフェース
    public void ActiveCharacter(bool active)
    {
        if (active)
        {
            GetNode<SePlayer>("/root/SePlayer").Play(SeName);

            if (StaticOnScreen)
            {
                GetNode<GameDialogLayer>("/root/DialogLayer").GetCurrentGameRoot().ReparentNode(this, StaticNodeName);
            }
        }
    }

    public void TerminateCharacter()
    {
        // スプライトアニメーションと効果音のみのデコレーションは
        // 直接QueueFree()を読んでも問題が発生しない場合に利用される。
        QueueFree();
    }
    #endregion
}
