using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour {
    public GatePart myGate;
    private static CastleController _instance;
    public static CastleController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CastleController>();
            }
            return _instance;
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpgradeGate()
    {

    }
}
