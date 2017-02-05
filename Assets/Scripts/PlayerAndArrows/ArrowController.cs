using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

    public bool arrowFlying = true;
    public LayerMask checkLayers;
    Rigidbody2D myRbd;
    public Transform arrowHead;
    public float ArrowCheckDistance = 0.2f;
    Vector2 vel = Vector2.zero;
    int ArrowUpdateRate = 20;
    int arrowDamage = 3;
    int tick;
    int frameCounter;
    // Use this for initialization
    void Start () {
        myRbd = GetComponent<Rigidbody2D>();
        frameCounter = 0;
        tick = 20 / ArrowUpdateRate;
	}

    GenericTrackWalker RecursevlyLookForScriptInParents(Transform _t)
    {
        GenericTrackWalker searchedScript = null;
        searchedScript = _t.gameObject.GetComponent<GenericTrackWalker>();
        if (searchedScript == null)
        {
            return RecursevlyLookForScriptInParents(_t.parent);
        }
        else
        {
            return searchedScript;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        frameCounter++;
        if (frameCounter % tick == 0)
        {
            if (arrowFlying)
            {
                vel = myRbd.velocity;
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg));
                RaycastHit2D hit = Physics2D.Raycast(arrowHead.position, vel.normalized, ArrowCheckDistance, checkLayers);

                if (hit.collider != null)
                {
                    FreezeArrow();
                    Debug.Log("ARROW HIT :" + hit.transform.gameObject.name);
                    if (hit.collider.gameObject.tag == "WalkerUnit")
                    {
                        IDamagableInterface target = UtilityScript.RecursevlyLookForInterface(hit.transform);
                        //GenericTrackWalker walkerScript = RecursevlyLookForScriptInParents(hit.collider.gameObject.transform);
                        this.transform.SetParent(hit.transform);

                        if (target != null)
                            target.ArrowHit(arrowDamage, hit.collider);
                    }
                    
                }
            }
        }

    }

    void FreezeArrow()
    {
        myRbd.velocity = Vector2.zero;
        myRbd.isKinematic = true;
        arrowFlying = false;
        
        //myRbd.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    void OnDrawGizmos()
    {
        Vector2 blah = vel.normalized * ArrowCheckDistance;
        Gizmos.DrawLine(arrowHead.position, arrowHead.position + new Vector3(blah.x, blah.y, 0f));
    }
}
