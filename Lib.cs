using Godot;

namespace teos;

/// <summary>
/// 便利関数
/// </summary>
public static partial class Lib
{
    /// <summary>
    /// PackedSceneを読み込む
    /// 取得できない場合はnullを返す。
    /// </summary>
    /// <param name="path">パス</param>
    /// <returns>Resource</returns>
    public static Resource GetPackedScene(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            GD.PrintErr("pathがnullまたはホワイトスペースです。リソースファイルを読み込めません。");
            return null;
        }

        // 存在はするがアクセス不能の場合、どうなるか確認していない。
        if (ResourceLoader.Exists(path))
        {
            return GD.Load(path);
        }

        GD.PrintErr($"リソースファイル{path}が存在しません。");
        return null;
    }
}
