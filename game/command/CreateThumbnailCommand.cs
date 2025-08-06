using Godot;
using teos.common.command;
using teos.game.system;

namespace teos.game.command;

/// <summary>
/// 画面サムネイル生成コマンド
/// </summary>
public partial class CreateThumbnailCommand : CommandRoot
{
    public override void ExecCommand(Node node, bool flag)
    {
        if (ExecFlag != flag)
        {
            return;
        }

        Image image = GetViewport().GetTexture().GetImage();
        image.Resize(128, 96);
        GetNode<GameDataManager>("/root/GameDataManager").SetThumbnail(image);
    }
}
