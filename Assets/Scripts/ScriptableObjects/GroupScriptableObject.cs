using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GroupScriptableObject : ScriptableObject {
    public WalkerScriptableObject[] walkerPattern;
    public int patternRepeatTime;
    public float minTimeBetweenPaterns;
}
