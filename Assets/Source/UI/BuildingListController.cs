using UnityEngine;

public class BuildingListController : MonoBehaviour
{

    public BuildingDatabaseAsset m_Database;
    public GameObject m_BuildingUITemplate;

    private void Start()
    {
        var container = GetComponent<RectTransform>();
        foreach (var building in m_Database.m_PlayerBuildings)
        {
            AddBuildingTemplate(container, building);
        }
    }

    private void AddBuildingTemplate(RectTransform parent, BuildingAsset asset)
    {
        var gobj = Instantiate(m_BuildingUITemplate, parent);
        var button = gobj.GetComponent<BuildingButtonController>();
        button.SetBuildingTemplate(asset);
    }

}
