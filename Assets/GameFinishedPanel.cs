using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinishedPanel : GenericCircularMenu {

    public override void Start()
    {
        base.Start();
        SetupButtonActions();
    }

    public override void SetupButtonActions()
    {
        base.SetupButtonActions();

        ButtonActions[0] = RestartQuickGame;
        ButtonActions[1] = GoToMainMenu;
    }

    public void GoToMainMenu()
    {
        ManagerFSM.InvokeEvent(enumEvents.OnGameExit);
    }

    public void RestartQuickGame()
    {
        ManagerFSM.InvokeEvent(enumEvents.OnQuickGame);
    }
}
