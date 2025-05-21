using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLine : MonoBehaviour
{
    public LineCollider lineCollider;

    public void Update()
    {
        LineCollisionScene.Instance.RemoveLineCollider(lineCollider);

        LineIntersectionResult result = new LineIntersectionResult();
        bool intersect = LineCollisionScene.Instance.IntersectLine(lineCollider.GetPoint(0).position, lineCollider.GetPoint(1).position, out result);

        if(intersect)
        {
            lineCollider.selectedColor = Color.red;
            lineCollider.defaultColor = Color.red;

            Debug.DrawLine(result.intersectPosition, result.intersectPosition + result.surfaceNormal, Color.magenta);
        }
        else
        {
            lineCollider.selectedColor = Color.yellow;
            lineCollider.defaultColor = Color.white;
        }
    }
}
