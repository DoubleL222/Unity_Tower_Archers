using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CastleUpgradablePart : MonoBehaviour {
    protected int currentLevel = 0;
    public virtual void Upgrade()
    {
        currentLevel++;
    }
}
