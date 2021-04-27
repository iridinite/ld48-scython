using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuController : MonoBehaviour
{

    public TMP_Text m_BuildingName;
    public TMP_Text m_BuildingDesc;
    public Image m_BuildingIcon;
    public TMP_Text m_UpkeepText;
    public TMP_Text m_UpgradeCost;
    public Button m_UpgradeButton;
    public GameObject m_UpgradeGroup;
    public GameObject m_DeconstructGroup;

    public GameObject m_BuildingListRoot;
    public GameObject m_VisualRoot;

    [Header("Triggers")]
    public GameObject m_TriggerUpgrade;
    public GameObject m_TriggerDeconstruct;

    private GridCell m_Target;
    private MapCursor m_Cursor;

    private void Awake()
    {
        // Hidden by default
        m_VisualRoot.SetActive(false);

        m_Cursor = FindObjectOfType<MapCursor>();
    }

    public void ShowCell(GridCell cell)
    {
        m_Target = cell;

        if (cell == null)
        {
            // Close
            m_BuildingListRoot.SetActive(true);
            m_VisualRoot.SetActive(false);
            m_Cursor.CancelSelection();
            return;
        }

        m_BuildingListRoot.SetActive(false);
        m_VisualRoot.SetActive(true);

        var asset = cell.m_BuildingAsset;
        m_BuildingName.text = asset.m_ShortName;
        m_BuildingDesc.text = asset.m_Description;
        m_BuildingIcon.sprite = asset.m_Icon;

        string upkeep = String.Empty;
        if (asset is BuildingAssetPowerConsumer consumer)
            upkeep += $"Requires <sprite name=Energy> {consumer.m_EnergyConsumption}";
        m_UpkeepText.text = upkeep;

        var upgrade = asset.m_Upgrade;
        if (upgrade != null)
        {
            m_UpgradeGroup.SetActive(true);
            m_UpgradeButton.interactable = false;
            m_UpgradeCost.text = $"Upgrade: <sprite name=Infernite> {upgrade.m_BuildCost}";
        }
        else
        {
            m_UpgradeGroup.SetActive(false);
        }

        m_DeconstructGroup.SetActive(asset.m_IsDeconstructable);
    }

    private void FixedUpdate()
    {
        if (m_Target == null)
            return;

        // Do this in FixedUpdate because money can change while UI is open
        var upgrade = m_Target.m_BuildingAsset.m_Upgrade;
        if (upgrade != null)
            m_UpgradeButton.interactable = GameSession.Instance.Money >= upgrade.m_BuildCost;
    }

    public void OnUpgrade()
    {
        var asset = m_Target.m_BuildingAsset;
        var upgrade = asset.m_Upgrade;
        Debug.Log($"Upgrading {asset.name} at {m_Target.m_MapPos} to {upgrade.name}");

        // Failsafe: check money
        if (GameSession.Instance.Money < upgrade.m_BuildCost)
            return;

        // Deduct the upgrade cost
        GameSession.Instance.Money -= upgrade.m_BuildCost;

        // Replace the building with empty space
        var map = m_Target.m_BuildingInstance.m_Map;
        m_Target.SetBuilding(map.transform, upgrade);

        // Reset the power network, since circumstances have changed
        m_Cursor.GetGameController().RequestEnergyNetworkRefresh();

        if (m_TriggerUpgrade != null)
            Instantiate(m_TriggerUpgrade, Vector3.zero, Quaternion.identity);

        // Re-open this menu on the same cell, to update info
        ShowCell(m_Target);
    }

    public void OnDeconstruct()
    {
        var asset = m_Target.m_BuildingAsset;
        if (!asset.m_IsDeconstructable)
            return; // Safeguard

        // Refund the building cost
        Debug.Log($"Deconstructing {asset.name} at {m_Target.m_MapPos}");
        GameSession.Instance.Money += asset.m_BuildCost;

        // Replace the building with empty space
        var map = m_Target.m_BuildingInstance.m_Map;
        m_Target.SetBuilding(map.transform, map.m_Database.m_TileEmpty);

        // Reset the power network, since circumstances have changed
        m_Cursor.GetGameController().RequestEnergyNetworkRefresh();

        if (m_TriggerDeconstruct != null)
            Instantiate(m_TriggerDeconstruct, Vector3.zero, Quaternion.identity);

        // Close this menu
        ShowCell(null);
    }

}
