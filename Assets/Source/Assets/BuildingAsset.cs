using UnityEngine;

[CreateAssetMenu(menuName = "Buildings/Basic")]
public class BuildingAsset : ScriptableObject
{

    [Header("UI")]
    public string m_ShortName;
    [TextArea(1, 4)] public string m_Description;
    public Sprite m_Icon;

    [Header("Gameplay")]
    public bool m_IsEmptySpace;
    public bool m_IsSelectable = true;
    public bool m_IsDeconstructable = true;
    public int m_BuildCost;
    public int m_UpkeepCost;
    public BuildingAsset m_Upgrade;

    [Header("Representation")]
    public GameObject m_Prefab;
    public GameObject m_PrefabGhost;
    public bool m_ShowCorruptionAnimations = true;

    [Header("Defense")]
    public float m_CorruptionSaturation;
    public bool m_CanBeInoperable;

}
 