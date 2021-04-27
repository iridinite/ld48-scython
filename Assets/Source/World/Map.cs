using UnityEngine;

public class Map : MonoBehaviour
{

    private GridCell[,] m_Map;
    private Transform m_Transform;

    public BuildingDatabaseAsset m_Database;

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_Map = new GridCell[Tuning.k_MapSize, Tuning.k_MapSize];

        InitializeMap();
    }

    private void InitializeMap()
    {
        // Initialize map array
        for (int y = 0; y < Tuning.k_MapSize; y++)
        {
            for (int x = 0; x < Tuning.k_MapSize; x++)
            {
                // Get the building that's supposed to be on this tile, according to the map data
                var asset = GetBuildingAssetForMapElement(MapList.ActiveMap[(Tuning.k_MapSize - y - 1) * Tuning.k_MapSize + x]);
                if (asset == null)
                    continue;

                // Initialize cell
                var cell = new GridCell
                {
                    m_MapPos = new Vector2Int(x, y)
                };
                cell.SetBuilding(m_Transform, asset);
                m_Map[x, y] = cell;
            }
        }
    }

    private BuildingAsset GetBuildingAssetForMapElement(int elem)
    {
        switch (elem)
        {
            case MapList.F:
                return m_Database.m_TileEmpty;
            case MapList.Q:
                return m_Database.m_TileEmptyBarren;
            case MapList.P:
                return m_Database.m_TilePortalCore;
            case MapList.S:
                return m_Database.m_TileSiphon;
            default:
                return null;
        }
    }

    public GridCell GetCellAt(int x, int y)
    {
        return GetCellAt(new Vector2Int(x, y));
    }

    public GridCell GetCellAt(Vector2Int mapPos)
    {
        return IsValid(mapPos) ? m_Map[mapPos.x, mapPos.y] : null;
    }

    public bool IsValid(int x, int y)
    {
        return IsValid(new Vector2Int(x, y));
    }

    public bool IsValid(Vector2Int mapPos)
    {
        return mapPos.x >= 0 
            && mapPos.y >= 0
            && mapPos.x < Tuning.k_MapSize
            && mapPos.y < Tuning.k_MapSize
            && m_Map[mapPos.x, mapPos.y] != null;
    }

    public static Vector2Int WorldToMap(float x, float y)
    {
        // Assuming that (0, 0, 0) is the center of portal tile (= the map center), and each tile is 1x1x1 in world units
        const int half_map = Tuning.k_MapSize / 2;
        return new Vector2Int(Mathf.RoundToInt(x) + half_map, Mathf.RoundToInt(y) + half_map);
    }

    public static Vector2Int WorldToMap(Vector3 world)
    {
        return WorldToMap(world.x, world.z);
    }

    public static Vector3 MapToWorld(Vector2Int mapPos)
    {
        const int half_map = Tuning.k_MapSize / 2;
        return new Vector3(mapPos.x - half_map, 0.0f, mapPos.y - half_map);
    }

}
