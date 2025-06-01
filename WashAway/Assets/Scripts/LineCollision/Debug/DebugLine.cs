using UnityEngine;

[RequireComponent(typeof(LineCollider))]
[ExecuteInEditMode]
public class DebugLine : MonoBehaviour
{
    private LineCollider lineCollider;
    private LineCollider[] lineColliders;

    public void Start()
    {
        lineCollider = GetComponent<LineCollider>();

        if(Application.isPlaying)
            LineCollisionScene.Instance.RemoveLineCollider(lineCollider);
    }

    public void Update()
    {
        lineColliders = FindObjectsOfType<LineCollider>();

        for (int i = 0; i < lineColliders.Length; i++)
        {
            LineCollider collider = lineColliders[i];

            if(collider != this)
            {
                LineIntersectionResult result = new LineIntersectionResult();
                bool intersect = collider.IntersectLine(lineCollider.GetPoint(0).position, lineCollider.GetPoint(1).position, out result);

                if (intersect)
                {
                    lineCollider.selectedColor = Color.red;
                    lineCollider.defaultColor = Color.red;

                    Debug.DrawLine(result.intersectPosition, result.intersectPosition + result.surfaceNormal, Color.magenta);
                }
                else
                {
                    lineCollider.selectedColor = Color.cyan;
                    lineCollider.defaultColor = Color.gray;
                }
            }
        }
    }
}
