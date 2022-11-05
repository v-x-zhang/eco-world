using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscDisplay : MonoBehaviour
{
    public float radius = 1;
    public Vector2 regionSize = Vector2.one;
    public int samplesBeforeRejection = 30;

    List<Vector2> points;

    public Vector3 regionCenter;
    public float displayRadius = 1f;

    private void OnValidate()
    {
        points = PoissonDiscSampling.GeneratePoints(radius, new Vector2(regionSize.x, regionCenter.y + regionSize.y), samplesBeforeRejection);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(regionCenter, regionSize);

        if(points != null)
        {
            foreach(Vector2 point in points)
            {
                Gizmos.DrawSphere(point, displayRadius);
            }
        }
    }
}
