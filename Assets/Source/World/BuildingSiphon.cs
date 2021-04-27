using UnityEngine;

public class BuildingSiphon : Building
{

    public override void TickCorruption()
    {
        if (!IsOperable())
            return;

        GameSession.Instance.Money += GameSession.Instance.PortalLayer.m_MoneyIncomePerSiphon;
    }

}
