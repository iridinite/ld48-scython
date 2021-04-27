using System;
using UnityEngine;

public class BuildingPowerConsumer : BuildingCable
{

    [NonSerialized] public bool m_IsPowered;

    public override void ResetPower()
    {
        base.ResetPower();

        m_IsPowered = false;
    }

    public bool IsPowered()
    {
        return m_IsPowered;
    }

}
