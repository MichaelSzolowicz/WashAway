using UnityEngine;

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
        lineCollidersInScene = FindObjectsOfType<LineCollider>();

        for (int i = 0; i < lineCollidersInScene.Length; i++)
        {
            LineCollider testCollider = lineCollidersInScene[i];

            if(testCollider != thisLineCollider)
            {
                LineIntersectionResult result = new LineIntersectionResult();
                bool intersect = testCollider.IntersectLine(thisLineCollider.GetPoint(0).position, thisLineCollider.GetPoint(1).position, out result);

                if (intersect)
                {
                    thisLineCollider.selectedColor = Color.red;
                    thisLineCollider.defaultColor = Color.red;

                    Debug.DrawLine(result.intersectPosition, result.intersectPosition + result.surfaceNormal, Color.magenta);
                }
                else
                {
                    thisLineCollider.selectedColor = Color.cyan;
                    thisLineCollider.defaultColor = Color.gray;
                }
            }
        }
    }
}
