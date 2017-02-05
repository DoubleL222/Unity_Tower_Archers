using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleController : MonoBehaviour, IDamagableInterface {
    public Collider2D gateCollider;
    public Collider2D bridgeCollider;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReceiveDamage(int damage)
    {
        throw new NotImplementedException();
    }

    public void NormalHit(int attackDamage, Collider2D col)
    {
        throw new NotImplementedException();
    }

    public void ArrowHit(int arrowDamage, Collider2D col)
    {
        throw new NotImplementedException();
    }
}
