using UnityEngine;

[CreateAssetMenu(menuName = "Buildings Database")]
public class BuildingDatabaseAsset : ScriptableObject
{

    [Header("System-managed Buildings")]
    public BuildingAsset m_TileEmpty;
    public BuildingAsset m_TileEmptyBarren;
    public BuildingAsset m_TilePortalCore;
    public BuildingAsset m_TileSiphon;

    [Header("Placeable Buildings")]
    public BuildingAsset[] m_PlayerBuildings;

}
