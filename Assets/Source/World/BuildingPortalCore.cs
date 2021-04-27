using UnityEngine;

public class BuildingPortalCore : Building
{

    private void LateUpdate()
    {
        // Ensure portal core always has its max corruption, to better show what's actually happening
        TickCorruption();
    }

    public override void TickCorruption()
    {
        // Generate corruption
        if (GameSession.Instance.IsCorruptionWaveActive)
            m_Cell.m_Corruption = GameSession.Instance.PortalLayer.m_CorruptionOutput;
    }

}
