using UnityEngine;
using UnityEngine.EventSystems;

public class MapCursor : MonoBehaviour
{

    public Material m_SpectralBuildingMaterial;
    public Color m_SpectralColorValid;
    public Color m_SpectralColorInvalid;

    public GameObject m_CursorVisual;
    public GameObject m_CursorSelectedPrefab;

    [Header("Triggers")]
    public GameObject m_TriggerConstruct;

    private Camera m_Camera;
    private Transform m_Transform;
    private Map m_Map;
    private GameController m_Ctrl;
    private UpgradeMenuController m_UpgradeController;
    private BuildingListFlyoutController m_ConstructionController;

    private Vector2Int m_MapPosition;
    private Vector2Int m_SelectedPosition;

    private BuildingAsset m_PlacementBuilding;
    private GameObject m_SpectralBuilding;
    private Transform m_SpectralBuildingTransform;
    private Transform m_CursorSelected;

    private void Awake()
    {
        m_Transform = GetComponent<Transform>();

        // what even is dependency injection lmao
        m_Camera = Camera.main;
        m_UpgradeController = FindObjectOfType<UpgradeMenuController>(true);
        m_ConstructionController = FindObjectOfType<BuildingListFlyoutController>();
        m_Map = FindObjectOfType<Map>();
        m_Ctrl = FindObjectOfType<GameController>();

        // Create new instance so we don't touch the one maintained by the editor
        m_SpectralBuildingMaterial = Instantiate(m_SpectralBuildingMaterial);

        m_CursorSelected = Instantiate(m_CursorSelectedPrefab, Vector3.zero, Quaternion.identity).GetComponent<Transform>();
        m_CursorSelected.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateHoveredTile();

        // Clicking a tile? (while not placing anything)
        if (Input.GetMouseButtonDown(0) && m_PlacementBuilding == null && !EventSystem.current.IsPointerOverGameObject())
        {
            if (m_Map.IsValid(m_MapPosition) && m_Map.GetCellAt(m_MapPosition).m_BuildingAsset.m_IsSelectable)
            {
                // Select this tile
                m_SelectedPosition = m_MapPosition;
                m_CursorSelected.gameObject.SetActive(true);
                m_CursorSelected.localPosition = Map.MapToWorld(m_SelectedPosition);
                m_UpgradeController.ShowCell(m_Map.GetCellAt(m_SelectedPosition));
            }
            else
            {
                // Deselect
                CancelSelection();
                m_UpgradeController.ShowCell(null);
            }
        }

        // Cancel with right-click REGARDLESS of whether the mouse is in a valid location or not, this is better UX
        if (Input.GetMouseButtonDown(1))
        {
            // The "Close Everything Please" button
            CancelBuildingPlacement();
            CancelSelection();
            m_UpgradeController.ShowCell(null);
            m_ConstructionController.HidePanel();
        }

        if (m_Map.IsValid(m_MapPosition))
        {
            // Move cursor object to correct map position
            m_CursorVisual.SetActive(true);
            var world = Map.MapToWorld(m_MapPosition);
            m_Transform.localPosition = world;

            // Handle placement
            if (m_PlacementBuilding != null)
            {
                // Move spectral building to the hovered tile as well
                m_SpectralBuilding.SetActive(true);
                m_SpectralBuildingTransform.localPosition = world;

                // Update color
                m_SpectralBuildingMaterial.SetColor("Tint", IsPlacementValid() ? m_SpectralColorValid : m_SpectralColorInvalid);

                // Place the building!
                if (Input.GetMouseButtonDown(0))
                    PlaceBuilding();
            }
        }
        else
        {
            // Hide cursor if no position is known
            m_CursorVisual.SetActive(false);
            m_SpectralBuilding?.SetActive(false);
        }
    }

    private bool IsPlacementValid()
    {
        // Verify that the cell is inside map borders
        if (!m_Map.IsValid(m_MapPosition))
            return false;

        // Verify that the cell isn't already occupied
        var cell = m_Map.GetCellAt(m_MapPosition);
        if (!cell.m_BuildingAsset.m_IsEmptySpace)
            return false;

        // Verify that the player can pay for the building
        if (GameSession.Instance.Money < m_PlacementBuilding.m_BuildCost)
            return false;

        return true;
    }

    private void PlaceBuilding()
    {
        // Verify placement
        if (!IsPlacementValid())
            return;

        // Deduct money
        GameSession.Instance.Money -= m_PlacementBuilding.m_BuildCost;

        // Spawn the building
        var cell = m_Map.GetCellAt(m_MapPosition);
        cell.SetBuilding(m_Map.transform, m_PlacementBuilding);
        m_Ctrl.RequestEnergyNetworkRefresh();

        if (m_TriggerConstruct != null)
            Instantiate(m_TriggerConstruct, Vector3.zero, Quaternion.identity);

        // Note: we do not exit placement mode because player may want to place more
    }

    public void CancelSelection()
    {
        m_SelectedPosition = new Vector2Int(-1, -1);
        m_CursorSelected.gameObject.SetActive(false);
    }

    public void CancelBuildingPlacement()
    {
        // Delete building visual
        if (m_SpectralBuilding != null)
            Destroy(m_SpectralBuilding);

        // Reset bookkeeping
        m_SpectralBuilding = null;
        m_SpectralBuildingTransform = null;
        m_PlacementBuilding = null;
    }

    public bool IsPlacementActive()
    {
        return m_PlacementBuilding != null;
    }

    public Vector2Int GetHoverPosition()
    {
        return m_MapPosition;
    }

    public GameController GetGameController()
    {
        return m_Ctrl;
    }

    public void BeginBuildingPlacement(BuildingAsset asset)
    {
        // Delete any previous spectral buildings
        CancelBuildingPlacement();

        // Create a new one
        m_PlacementBuilding = asset;
        m_SpectralBuilding = Instantiate(asset.m_PrefabGhost, Map.MapToWorld(m_MapPosition), Quaternion.identity);
        m_SpectralBuildingTransform = m_SpectralBuilding.GetComponent<Transform>();

        // Apply transparent shader to all renderers in the building
        var renderers = m_SpectralBuilding.GetComponentsInChildren<MeshRenderer>();
        foreach (var r in renderers)
            r.sharedMaterial = m_SpectralBuildingMaterial;
    }

    private void UpdateHoveredTile()
    {
        // If mouse is over UI, make the cursor invalid so we can't accidentally "click through" on world-space things
        if (EventSystem.current.IsPointerOverGameObject())
        {
            m_MapPosition = new Vector2Int(-1, -1);
            return;
        }

        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);

        // Intersect with ground
        Plane ground = new Plane(Vector3.up, 0.0f);
        if (!ground.Raycast(ray, out float enter))
            return;

        // Find corresponding world pos
        Vector3 ground_point = ray.GetPoint(enter);

        // Transform to map pos
        m_MapPosition = Map.WorldToMap(ground_point);
    }

}
