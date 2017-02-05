using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoCollider : MonoBehaviour {
    public BoxCollider2D col;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if(col != null)
            Gizmos.DrawWireCube(transform.position, new Vector3(col.size.x/2f, col.size.y / 2f, 1));
    }
}
