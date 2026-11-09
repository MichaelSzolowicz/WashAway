using UnityEngine;

/// <summary>
/// Set points in a Line Renderer to match a Line Collider.
/// Requires both a Line Renderer and Line Collider component.
/// </summary>
[ExecuteInEditMode]
public class LinerColliderRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private LineCollider lineCollider;

    private void OnEnable()
    {
        UpdateLineRenderer();
        lineCollider.onUpdatePoints += UpdateLineRenderer;
    }

    private void Update()
    {
        if(lineCollider == null || lineRenderer == null)
        {
            UpdateLineRenderer();
        }
        if (transform.hasChanged)
        {
            UpdateLineRenderer();
            transform.hasChanged = false;
        }
    }

    private void UpdateLineRenderer()
    {
        if(!HasRequiredComponents()) return;

        Vector3[] points = new Vector3[lineCollider.NumPoints];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = lineCollider.GetPointWorldPosition(i);
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    private bool HasRequiredComponents()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        if (lineCollider == null) lineCollider = GetComponent<LineCollider>();
        return lineRenderer && lineCollider;
    }

    private void OnDisable()
    {
        lineCollider.onUpdatePoints -= UpdateLineRenderer;
    }
}
