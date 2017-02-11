using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoTest : MonoBehaviour {
    public SpriteRenderer sr;
    Vector2[] lal;
    // Use this for initialization
    EnemyWalkingTrack track;
    public GenericTrackWalker e;
    public GenericTrackWalker f;
    void Start () {
        //track = new EnemyWalkingTrack(sr.sprite, EnemyWalkingTrackSide.Left);
        //FRIENDLY WOULD BE SPAWNED LIKE THIS
        //f.FollowTrack(track.Points-1, track, -1);
        //ENEMY LIKE THIS
        //e.FollowTrack(0, track, 1);
    }

    void FindPoints()
    {

    }
    void OnDrawGizmos()
    {
        if (track != null)
        {
            foreach (Vector2 v in track.walkingPoints)
            {
                Gizmos.DrawSphere(v, 0.1f);
            }
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
