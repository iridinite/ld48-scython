using System;
using UnityEngine;

public class BuildingPowerGenerator : BuildingCable
{

    [NonSerialized] public int m_PowerAvailable;

    private void Start()
    {
        // So readout is accurate even when portal is closed / ticks don't happen
        ResetPower();
    }

    public override void ResetPower()
    {
        base.ResetPower();

        BuildingAssetPowerGenerator asset = (BuildingAssetPowerGenerator)m_Cell.m_BuildingAsset;
        m_PowerAvailable = asset.m_EnergyOutput;
    }

}
