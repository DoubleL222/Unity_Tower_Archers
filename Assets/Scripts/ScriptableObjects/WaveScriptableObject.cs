using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WaveScriptableObject : ScriptableObject
{
    public GroupScriptableObject[] Groups;
    public float minTimeBetweenGroups;
}
