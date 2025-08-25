using UnityEngine;

[RequireComponent(typeof(LineCollider))]
[ExecuteInEditMode]
public class DebugLine : MonoBehaviour
{
    private LineCollider thisLineCollider;
    private LineCollider[] lineCollidersInScene;

    private Color[] defaultColors = { Color.white, Color.white };

    public void OnValidate()
    {
        thisLineCollider = GetComponent<LineCollider>();
        defaultColors[0] = thisLineCollider.selectedColor;
        defaultColors[1] = thisLineCollider.deselectedColor;
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
                bool intersect = testCollider.IntersectLine(thisLineCollider.GetPointWorldPosition(0), thisLineCollider.GetPointWorldPosition(1), out result);

                if (intersect)
                {
                    thisLineCollider.deselectedColor = Color.red;
                    thisLineCollider.selectedColor = Color.red;

                    Debug.DrawLine(result.intersectPosition, result.intersectPosition + result.surfaceNormal, Color.magenta);
                }
                else
                {
                    thisLineCollider.selectedColor = defaultColors[0];
                    thisLineCollider.deselectedColor = defaultColors[1];
                }
            }
        }
    }
}
