using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityScript  {

    public static Vector3 V2toV3(Vector2 v)
    {
        return new Vector3(v.x, v.y, 0);
    }

    public static Vector2 V3toV2(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static IDamagableInterface RecursevlyLookForInterface(Transform _t)
    {
        IDamagableInterface searchedScript = null;
        searchedScript = _t.gameObject.GetComponent<IDamagableInterface>();
        if (searchedScript == null)
        {
            return RecursevlyLookForInterface(_t.parent);
        }
        else
        {
            return searchedScript;
        }
    }

    public static GenericTrackWalker RecursevlyLookForWalkerClass(Transform _t)
    {
        GenericTrackWalker searchedScript = null;
        searchedScript = _t.gameObject.GetComponent<GenericTrackWalker>();
        if (searchedScript == null)
        {
            return RecursevlyLookForWalkerClass(_t.parent);
        }
        else
        {
            return searchedScript;
        }
    }

}
