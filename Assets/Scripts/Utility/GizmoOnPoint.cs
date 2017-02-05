using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoOnPoint : MonoBehaviour {

    public Color gizmoColor = Color.yellow;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(this.transform.position, 0.5f);
    }
}
