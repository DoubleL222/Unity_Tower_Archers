using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleHeartScript : MonoBehaviour {
    public CastleController castleController;
    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("On trigger EnTER HEART");
        if ((UnitSpawnManager.instance.enemyLayerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            Debug.Log("Enemy Got To heart");
           // IDamagableInterface unit = UtilityScript.RecursevlyLookForInterface(other.transform);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("On trigger STAY HEART");
        if ((UnitSpawnManager.instance.enemyLayerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            Debug.Log("Enemy Got To heart");
            // IDamagableInterface unit = UtilityScript.RecursevlyLookForInterface(other.transform);
        }
    }
}
