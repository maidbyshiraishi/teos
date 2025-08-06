using Godot;

namespace teos.common.tilemap;

/// <summary>
/// 前面タイルマップレイヤーの切り抜きポリゴン
/// </summary>
public partial class ObservationHole : Polygon2D
{
    private static readonly Vector2 GreatDistance = new(-2000f, -2000f);

    [Export]
    public float ObservationHoleRadius { get; set; } = 80f;

    [Export]
    public int ObservationHoleVertexCount { get; set; } = 12;

    private TileMapLayer _target;

    public override void _Ready()
    {
        _target = FindTileMapLayer();
        MakeObservationHole();
    }

    private TileMapLayer FindTileMapLayer()
    {
        foreach (Node node in GetChildren())
        {
            if (node is TileMapLayer tileMapLayer)
            {
                return tileMapLayer;
            }
        }

        return null;
    }

    private void MakeObservationHole()
    {
        Vector2[] list = new Vector2[ObservationHoleVertexCount];
        Vector2 line = new(ObservationHoleRadius, 0f);
        int length = list.Length;
        float angle = Mathf.DegToRad(360f / length);
        list[0] = line;

        for (int i = 1; i < length; i++)
        {
            line = line.Rotated(angle);
            list[i] = line;
        }

        Polygon = list;
    }

    public void ManageObservationHole(Vector2 position)
    {
        if (_target is null || TileMapManager.GetTileData((Vector2I)position, _target) is null)
        {
            Offset = position + GreatDistance;
            return;
        }

        Offset = position;
    }
}
