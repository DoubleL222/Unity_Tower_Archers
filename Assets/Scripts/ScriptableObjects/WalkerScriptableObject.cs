using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WalkerScriptableObject : ScriptableObject {

    public GameObject Prefab;
    public int startingHealth;
    public int startingDamage;
    public float startingSpeed;
    public float startingAttackSpeed = 1.0f;
    public WalkerType type;
}
