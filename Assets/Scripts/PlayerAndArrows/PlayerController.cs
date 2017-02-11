using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerController : MonoBehaviour {
    // Use this for initialization
    public Transform arrowSpawn;
    private float arrowShootCooldown = 0.5f;
    private float nextShootTime = float.MinValue;
	void Start () {
		
	}
    public void FireArrow(GameObject _arrowPrefab, Vector3 _arrowSpawnLocation, Vector2 _direction, float _arrowSpeed)
    {
        if (Time.time > nextShootTime)
        {
            GameObject arw = Instantiate(_arrowPrefab, _arrowSpawnLocation, Quaternion.Euler(0, 0, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg)) as GameObject;
            Rigidbody2D rbdArw = arw.GetComponent<Rigidbody2D>();
            rbdArw.velocity = new Vector2(_direction.x, _direction.y) * _arrowSpeed;
            nextShootTime = Time.time + arrowShootCooldown;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
