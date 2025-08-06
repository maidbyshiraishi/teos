using Godot;

namespace teos.common.tilemap;

/// <summary>
/// タイルマップ制御
/// </summary>
public partial class TileMapManager : TileMapLayer
{
    public TileData GetTileData(Vector2I coords)
    {
        return GetTileData(coords, this);
    }

    public bool GetCustomDataAsBool(Vector2I coords, string name)
    {
        return GetCustomDataAsBool(coords, name, this);
    }

    public int GetCustomDataAsInt(Vector2I coords, string name)
    {
        return GetCustomDataAsInt(coords, name, this);
    }

    public Variant GetCustomDataAsVariant(Vector2I coords, string name)
    {
        return GetCustomDataAsVariant(coords, name, this);
    }

    public void RemoveBlock(Vector2I coords)
    {
        RemoveBlock(coords, this);
    }


    /// <summary>
    /// タイルデータを取得する
    /// </summary>
    /// <param name="coords">座標</param>
    /// <param name="tileMapLayer">TileMapLayer</param>
    /// <returns>TileData</returns>
    public static TileData GetTileData(Vector2I coords, TileMapLayer tileMapLayer)
    {
        Vector2I local = tileMapLayer.LocalToMap(coords);
        TileData tiledataHead = tileMapLayer.GetCellTileData(local);
        return tiledataHead;
    }

    /// <summary>
    /// 対象座標のタイルに設定されたカスタムデータを
    /// bool値として取得する
    /// </summary>
    /// <param name="coords">座標</param>
    /// <param name="name">カスタムデータ名</param>
    /// <param name="tileMapLayer">TileMapLayer</param>
    /// <returns>カスタムデータ</returns>
    public static bool GetCustomDataAsBool(Vector2I coords, string name, TileMapLayer tileMapLayer)
    {
        Variant v = GetCustomDataAsVariant(coords, name, tileMapLayer);
        return v.VariantType is Variant.Type.Bool && v.AsBool();
    }

    /// <summary>
    /// 対象座標のタイルに設定されたカスタムデータを
    /// int値として取得する
    /// </summary>
    /// <param name="coords">座標</param>
    /// <param name="name">カスタムデータ名</param>
    /// <param name="tileMapLayer">TileMapLayer</param>
    /// <returns>カスタムデータ</returns>
    public static int GetCustomDataAsInt(Vector2I coords, string name, TileMapLayer tileMapLayer)
    {
        Variant v = GetCustomDataAsVariant(coords, name, tileMapLayer);
        return v.VariantType is Variant.Type.Int ? v.AsInt32() : 0;
    }

    /// <summary>
    /// 対象座標のタイルに設定されたカスタムデータを
    /// Variant値として取得する
    /// </summary>
    /// <param name="coords">座標</param>
    /// <param name="name">カスタムデータ名</param>
    /// <param name="tileMapLayer">TileMapLayer</param>
    /// <returns>カスタムデータ</returns>
    public static Variant GetCustomDataAsVariant(Vector2I coords, string name, TileMapLayer tileMapLayer)
    {
        Vector2I local = tileMapLayer.LocalToMap(coords);
        TileData tileData = tileMapLayer.GetCellTileData(local);
        return tileData is null ? new() : tileData.GetCustomData(name);
    }

    /// <summary>
    /// 指定座標のブロックを削除する
    /// </summary>
    /// <param name="coords">座標</param>
    /// <param name="tileMapLayer">TileMapLayer</param>
    public static void RemoveBlock(Vector2I coords, TileMapLayer tileMapLayer)
    {
        tileMapLayer.EraseCell(tileMapLayer.LocalToMap(coords));
    }
}
