using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class AirConsoleMessageHandler : MonoBehaviour {

    void Start()
    {
        SetupAirconsoleCallbacks();
    }

    void SetupAirconsoleCallbacks()
    {
        AirConsole.instance.onConnect += PlayerConnected;
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onDisconnect += PlayerDisconected;
    }

    void PlayerConnected(int _deviceId)
    {
        AirConsolePlayerKeeper.instance.OnPlayerConnected(_deviceId);
        
    }

    void PlayerDisconected(int _deviceId)
    {
        AirConsolePlayerKeeper.instance.OnPlayerDisconected(_deviceId);
        // AirConsolePlayerKeeper.instance.OnPlayerConnected(_deviceId);
        //  GameObject newPlayer = Instantiate(playerPrefab, spawnPoints[activePlayers.Count].position, Quaternion.identity) as GameObject;
        //  p.playerController = newPlayer.GetComponent<PlayerController>();

    }

    void OnMessage(int from, JToken data)
    {
        // Debug.Log("Got message:"+data.ToString()+"; from :" + from);
        AirConsolePlayer player = AirConsolePlayerKeeper.instance.PlayerByDeviceId(from);
        if (player != null)
            ParseInputData(data, player);
    }

    void ParseInputData(JToken data, AirConsolePlayer player)
    {
        if (data["0"] != null)
        {
            HandleButtonA(player);
        }
        else if (data["1"] != null)
        {
            HandleButtonB(player);
        }
        else if (data["joystick-right"] != null)
        {
            bool pressed = player.holdinJoystick;
            float x = player.prevjoystickPos.x;
            float y = player.prevjoystickPos.y;

            if (data["joystick-right"]["pressed"] != null)
            {
                pressed = (bool)data["joystick-right"]["pressed"];
            }
            if (data["joystick-right"]["message"]["x"] != null)
            {
                x = (float)data["joystick-right"]["message"]["x"];
            }
            if (data["joystick-right"]["message"]["y"] != null)
            {
                y = (float)data["joystick-right"]["message"]["y"];
            }
            HandleJoystickChange(player, pressed, new Vector2(x, y));
        }
    }

    void HandleButtonA(AirConsolePlayer player)
    {
        Debug.Log("BUTTON A");
        if (ManagerFSM.InState(enumStates.MainMenu) || ManagerFSM.InState(enumStates.UpgradeMenu) || ManagerFSM.InState(enumStates.GameFinished))
        {
            GuiManager.instance.HandleAButton();
        }
        else if (ManagerFSM.InState(enumStates.Wave))
        {
            Debug.Log("SENDING CHANGE TO GAME CONTROLLER");
            GameController.instance.HandleButtonA(player);
        }
       
      //  GameController.instance.HandleButtonA(player);
    }

    void HandleButtonB(AirConsolePlayer player)
    {

    }

    void HandleJoystickChange(AirConsolePlayer player, bool pressed, Vector2 pos)
    {
        if (ManagerFSM.InState(enumStates.MainMenu) || ManagerFSM.InState(enumStates.UpgradeMenu) || ManagerFSM.InState(enumStates.GameFinished))
        {
            GuiManager.instance.HandleJoystickChange(player, pressed, pos);
        }
        else if (ManagerFSM.InState(enumStates.Wave))
        {
            Debug.Log("SENDING CHANGE TO GAME CONTROLLER");
            GameController.instance.HandleJoystickChange(player, pressed, pos);
        }
    }
}
