using UnityEngine;

[CreateAssetMenu(menuName = "Map Info")]
public class MapInfoAsset : ScriptableObject
{

    [Header("UI")]
    public string m_ShortName;
    [TextArea(1, 4)] public string m_Description;
    public Texture m_Preview;

    [Header("System")]
    [Min(0)] public int m_MapIndex;

}
 