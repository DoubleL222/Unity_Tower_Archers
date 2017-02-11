using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : GenericCircularMenu
{
    public override void Start()
    {
        base.Start();
        SetupButtonActions();
    }

    public override void SetupButtonActions()
    {
        base.SetupButtonActions();

        ButtonActions[3] = ContinueFromUpgrade;
    }

    public void ContinueFromUpgrade()
    {
        ManagerFSM.InvokeEvent(enumEvents.OnNextWave);
        CastleController.instance.UpgradeGate();
    }
}
