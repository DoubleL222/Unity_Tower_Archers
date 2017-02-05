using UnityEngine;
using System.Collections;

public class AirConsolePlayer  {

    public Vector2 prevjoystickPos = Vector2.zero;
    public bool holdinJoystick;
    int deviceId;
    public PlayerController playerController;
    public AirConsolePlayer(int _deviceId)
    {
        deviceId = _deviceId;
        holdinJoystick = false;
    }

    public int GetDeviceId()
    {
        return deviceId;
    }

}
