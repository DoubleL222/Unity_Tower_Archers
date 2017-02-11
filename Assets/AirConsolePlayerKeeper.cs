using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using System.Linq;
using Newtonsoft.Json.Linq;

public class AirConsolePlayerKeeper : MonoBehaviour {

    private static AirConsolePlayerKeeper _instance;
    public static AirConsolePlayerKeeper instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AirConsolePlayerKeeper>();
            }
            return _instance;
        }
    }
    List<AirConsolePlayer> activePlayers;
    List<AirConsolePlayer> disconectedPlayers;

    public void Start()
    {
        RefreshPlayerList();    
    }

    void RefreshPlayerList()
    {
        activePlayers = new List<AirConsolePlayer>();
        disconectedPlayers = new List<AirConsolePlayer>();
    }

    public void OnPlayerConnected(int _deviceId)
    {
        AirConsolePlayer p = new AirConsolePlayer(_deviceId);
        activePlayers.Add(p);
        Debug.Log("///AirConsolePlayerKeeper:::" + _deviceId + "And player nubmer: " + AirConsole.instance.ConvertDeviceIdToPlayerNumber(_deviceId) + " connected");
       // GameController.instance.InstantiateBowmanForPlayer(p);
    }

    public void OnPlayerDisconected(int _deviceId)
    {
        AirConsolePlayer temp = activePlayers.Where(x => x.GetDeviceId() == _deviceId).First();
        disconectedPlayers.Add(temp);
        activePlayers.RemoveAll(x => x.GetDeviceId() == _deviceId);
        Debug.Log("///AirConsolePlayerKeeper:::" + _deviceId + " disconected: ");
    }

    public AirConsolePlayer PlayerByDeviceId(int _deviceID)
    {
        return activePlayers.Where(x => x.GetDeviceId() == _deviceID).First();
    }

    public List<AirConsolePlayer> GetAllActivePlayers()
    {
        return activePlayers;
    }
}
