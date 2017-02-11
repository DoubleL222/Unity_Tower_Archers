using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum EnemyWalkingTrackSide
{
    Left,
    Right
}

public class EnemyWalkingTrack {
    public List<Vector2> walkingPoints;
    float minXDiff = 0.1f;
    public EnemyWalkingTrackSide side;
    Vector3 spritePosition;
    Vector3 spriteScale;

    public int Points
    {
        get
        {
            return walkingPoints.Count;
        }
    }
    public EnemyWalkingTrack(Sprite _sprite, EnemyWalkingTrackSide _side, Transform _spriteTransform)
    {
        side = _side;
        spritePosition = _spriteTransform.position;
        spriteScale = _spriteTransform.localScale;
        walkingPoints = GetWalkingPointsFromSprite(_sprite);
    }

    List<Vector2> GetWalkingPointsFromSprite(Sprite _sprite)
    {
        List<Vector2> wPoints = new List<Vector2>();
        Vector3 max = _sprite.bounds.max;
        Vector3 min = _sprite.bounds.min;
        Vector2[] vertices = _sprite.vertices;

        for (int j = 0; j < vertices.Length; j++)
        {
            vertices[j] = new Vector2(vertices[j].x * spriteScale.x, vertices[j].y * spriteScale.y);
            Debug.Log("Sprite position in for loop " + spritePosition);
            vertices[j] = vertices[j] + UtilityScript.V3toV2(spritePosition);
         //   Debug.Log("New vertex prev pos: " + vertices[j] + "; added vector : " + UtilityScript.V3toV2(spritePosition) + " new position: " + vertices[j] + UtilityScript.V3toV2(spritePosition));
        }
        Dictionary<float, float> points = new Dictionary<float, float>();
        foreach (Vector2 v in vertices)
        {
            if (points.ContainsKey(v.x))
            {
                if (points[v.x] < v.y)
                {
                    points[v.x] = v.y;
                }
            }
            else
            {
                // Debug.Log(_sprite.bounds.min);
                if (v.y != min.y)
                {
                    bool tooClose = false;
                    foreach (KeyValuePair<float, float> kvp in points)
                    {
                        if (Mathf.Abs(kvp.Key - v.x) < minXDiff)
                            tooClose = true;
                    }
                    if(!tooClose)
                        points.Add(v.x, v.y);
                }
            }
        }
        foreach (KeyValuePair<float, float> kvp in points)
        {
            wPoints.Add( new Vector2(kvp.Key, kvp.Value));
        }
        if (side == EnemyWalkingTrackSide.Left)
        {
            wPoints = wPoints.OrderBy(v => v.x).ToList();
        }
        else
        {
            wPoints = wPoints.OrderByDescending(v => v.x).ToList();
        }
        return wPoints;
    }

}
