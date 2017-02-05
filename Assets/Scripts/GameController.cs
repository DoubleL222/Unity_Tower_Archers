using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.Linq;
using System;
//ENUM FOR EVENTS
public enum enumEvents
{
    AtoB,
    BtoA,
    BtoC,
    CtoB,
    AtoC,
    CtoA
}
//ENUM FOR STATES
public enum enumStates
{
    A,
    B,
    C
}
public class GameController : MonoBehaviour {
    public List<Transform> spawnPoints;

    List<AirConsolePlayer> activePlayers;
    List<AirConsolePlayer> disconectedPlayers;
    public PlayerController player1;

    [Header("References")]
    public GameObject arrowPrefab;
    public GameObject playerPrefab;

    [Header("Settings")]
    public float arrowSpeed;

    void OnEnable()
    {
        SetupAirconsoleCallbacks();
    }

    void OnDisable()
    {

    }

	// Use this for initialization
	void Start () {
        activePlayers = new List<AirConsolePlayer>();
        disconectedPlayers = new List<AirConsolePlayer>();
    }

    void SetupAirconsoleCallbacks()
    {
        AirConsole.instance.onConnect += PlayerConnected;
        AirConsole.instance.onMessage += OnMessage;
    }

    void PlayerConnected(int _deviceId)
    {
        AirConsolePlayer p = new AirConsolePlayer(_deviceId);
        GameObject newPlayer = Instantiate(playerPrefab, spawnPoints[activePlayers.Count].position, Quaternion.identity) as GameObject;
        p.playerController = newPlayer.GetComponent<PlayerController>();
        activePlayers.Add(p);
        Debug.Log("Player with dID " + _deviceId +  "And player nubmer: "+ AirConsole.instance.ConvertDeviceIdToPlayerNumber(_deviceId) +" connected");
    }

    void OnMessage(int from, JToken data)
    {
        // Debug.Log("Got message:"+data.ToString()+"; from :" + from);
        var player = activePlayers.Where(p => p.GetDeviceId() == from);
        if(player.Count() > 0)
            ParseInputData(data, player.First());
    }

    void ParseInputData(JToken data, AirConsolePlayer player)
    {
        if (data["0"] != null)
        {
            Debug.Log("Button 0");
        }
        else if (data["1"] != null)
        {
            Debug.Log("Button 1");
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
            HandleJoystickChange(player, pressed, new Vector2(x,y));
        }
    }

    void HandleButtonA(AirConsolePlayer player)
    {

    }

    void HandleButtonB(AirConsolePlayer player)
    {

    }

    void HandleJoystickChange(AirConsolePlayer player, bool pressed, Vector2 pos)
    {
        if (player.holdinJoystick)
        {
            if (pressed)
            {
                //Debug.Log("holding");
            }
            else
            {
               // Debug.Log("Previous joystick position " + player.prevjoystickPos);
               // Debug.Log("let go with joystick position "+ pos);
                FireArrow(player, pos);
            }
        }
        else
        {
            if (pressed)
            {
               // Debug.Log("started holding");
            }
            else
            {
               // Debug.Log("What the fuck!?");
            }
        }
        player.holdinJoystick = pressed;
        player.prevjoystickPos = pos;
    }

    void FireArrow(AirConsolePlayer _player, Vector2 _lastJoystickPosition)
    {
        Vector2 dir = new Vector2(-_lastJoystickPosition.x, _lastJoystickPosition.y);
        _player.playerController.FireArrow(arrowPrefab, _player.playerController.arrowSpawn.position, dir, arrowSpeed);
        //player1.FireArrow(arrowPrefab, player1.arrowSpawn.position, dir, arrowSpeed);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
