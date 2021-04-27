using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonController : MonoBehaviour
{

    [NonSerialized] public BuildingAsset m_BuildingToPlace;

    public TMP_Text m_TextName;
    public TMP_Text m_TextCost;
    public Image m_Icon;

    private MapCursor m_Cursor;
    private BuildingTooltipController m_Tooltip;

    private void Awake()
    {
        m_Cursor = FindObjectOfType<MapCursor>();
        m_Tooltip = FindObjectOfType<BuildingTooltipController>();
    }

    public void SetBuildingTemplate(BuildingAsset asset)
    {
        m_BuildingToPlace = asset;
        m_TextName.text = asset.m_ShortName;
        m_TextCost.text = $"<sprite name=Infernite> {asset.m_BuildCost}";
        m_Icon.sprite = asset.m_Icon;
    }

    public void OnClick()
    {
        m_Cursor.BeginBuildingPlacement(m_BuildingToPlace);
    }

    public void OnShowTooltip()
    {
        string tooltip = m_BuildingToPlace.m_Description;
        if (m_BuildingToPlace is BuildingAssetPowerConsumer consumer)
            tooltip += $"\n\nRequires <sprite name=Energy> {consumer.m_EnergyConsumption}";

        m_Tooltip.ShowTooltip(tooltip);
    }

    public void OnHideTooltip()
    {
        m_Tooltip.HideTooltip();
    }

}
