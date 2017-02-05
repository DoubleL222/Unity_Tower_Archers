using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class TestLogic : MonoBehaviour {

    void Start()
    {
        AirConsole.instance.onMessage += OnMessage;
    }

    void OnMessage(int from, JToken data)
    {
        Debug.Log("Got message from :" + from);
        AirConsole.instance.Message(from, "Full of pixels!");
    }
}
