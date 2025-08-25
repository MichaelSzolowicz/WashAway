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
        defaultColors[0] = thisLineCollider.SelectedColor;
        defaultColors[1] = thisLineCollider.DeselectedColor;
    }

    public void Update()
    {
        lineCollidersInScene = FindObjectsOfType<LineCollider>();
        bool foundIntersect = false;

        for (int i = 0; i < lineCollidersInScene.Length; i++)
        {
            LineCollider testCollider = lineCollidersInScene[i];

            if(testCollider != thisLineCollider)
            {
                LineIntersectionResult result = new LineIntersectionResult();
                bool intersect = testCollider.IntersectLine(thisLineCollider.GetPointWorldPosition(0), thisLineCollider.GetPointWorldPosition(1), out result);

                if (intersect)
                {
                    foundIntersect = true;


                    Debug.DrawLine(result.intersectPosition, result.intersectPosition + result.surfaceNormal, Color.magenta);
                }
                else
                {

                }
            }
        }

        if(foundIntersect )
        {
            thisLineCollider.DeselectedColor = Color.red;
            thisLineCollider.SelectedColor = Color.red;
        }
        else
        {
            thisLineCollider.SelectedColor = defaultColors[0];
            thisLineCollider.DeselectedColor = defaultColors[1];
        }
    }
}
