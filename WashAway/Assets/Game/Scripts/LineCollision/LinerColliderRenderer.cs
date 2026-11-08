using UnityEngine;

/// <summary>
/// Set points in a Line Renderer to match a Line Collider.
/// Requires both a Line Renderer and Line Collider component.
/// </summary>
[ExecuteInEditMode]
public class LinerColliderRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private LineCollider lineCollider;

    private void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineCollider = GetComponent<LineCollider>();

        lineCollider.onUpdatePoints += UpdateLineRenderer;
    }

    private void UpdateLineRenderer()
    {
        if (lineRenderer == null) return;

        Vector3[] points = new Vector3[lineCollider.NumPoints];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = lineCollider.GetPointWorldPosition(i);
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    private void OnDisable()
    {
        lineCollider.onUpdatePoints -= UpdateLineRenderer;
    }
}
