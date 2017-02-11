using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : GenericCircularMenu {

    public override void Start()
    {
        base.Start();
        SetupButtonActions();
    }

    public override void SetupButtonActions()
    {
        base.SetupButtonActions();
        ButtonActions[0] = StartGameMenuItem;
    }

    public void StartGameMenuItem()
    {
        ManagerFSM.InvokeEvent(enumEvents.OnQuickGame);
    }
}
