using UnityEngine;

/// <summary>
/// Horribly inefficient little thing that shows line intersections in editor. Do not include in a shipping scene.
/// </summary>
[RequireComponent(typeof(LineCollider))]
[ExecuteInEditMode]
public class DebugLine : MonoBehaviour
{
    private LineCollider thisLineCollider;
    private LineCollider[] lineCollidersInScene;

    public void OnValidate()
    {
        thisLineCollider = GetComponent<LineCollider>();
    }

    public void Update()
    {
        if(thisLineCollider.NumPoints < 2) return; 

        lineCollidersInScene = FindObjectsOfType<LineCollider>();

        bool foundIntersect = false;
        for (int i = 0; i < lineCollidersInScene.Length; i++)
        {
            LineCollider testCollider = lineCollidersInScene[i];

            if (testCollider != thisLineCollider)
            {
                LineIntersectionResult result = new LineIntersectionResult();
                bool intersect = testCollider.IntersectLine(thisLineCollider.GetPointWorldPosition(0), thisLineCollider.GetPointWorldPosition(1), out result);

                if (intersect)
                {
                    foundIntersect = true;

                    Vector3 start = result.intersectPosition;
                    start.z = transform.position.z;
                    Debug.DrawLine(start, start + result.surfaceNormal, Color.magenta);
                }
            }
        }

        if(foundIntersect)
        {
            thisLineCollider.DeselectedColor = Color.red;
            thisLineCollider.SelectedColor = Color.red;
        }
        else
        {
            thisLineCollider.SelectedColor = Color.yellow;
            thisLineCollider.DeselectedColor = Color.grey;
        }
    }
}


