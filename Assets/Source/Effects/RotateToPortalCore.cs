using UnityEngine;

public class RotateToPortalCore : MonoBehaviour
{

    private void Start()
    {
        // Find a portal core
        Vector3 target = Map.MapToWorld(FindPortalCore());

        // Rotate this transform towards it
        var tf = GetComponent<Transform>();
        //Vector3 origin = tf.position;
        //tf.localRotation = Quaternion.Euler(0f, Quaternion.);
        tf.LookAt(target, Vector3.up);
    }

    private Vector2Int FindPortalCore()
    {
        var building = GetComponentInParent<Building>();
        var map = building.m_Map;
        for (int y = 0; y < Tuning.k_MapSize; y++)
            for (int x = 0; x < Tuning.k_MapSize; x++)
                if (map.GetCellAt(x, y)?.m_BuildingInstance is BuildingPortalCore)
                    return new Vector2Int(x, y);

        Debug.LogError("Can't find portalcore for siphon rotation");
        return Vector2Int.zero;
    }

}
