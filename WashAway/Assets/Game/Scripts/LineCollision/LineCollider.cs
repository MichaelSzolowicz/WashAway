using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LineCollider : MonoBehaviour, ILineColliderInterface
{
    [SerializeField] private List<LinePoint> points = new List<LinePoint>();

    [System.Serializable]
    private class LineColliderAppearance
    {
        public Color deselectedColor;
        public Color selectedColor;
        public float normalLength;
        public bool visibleInGame;
        public bool visibleInEditor;

        public LineColliderAppearance()
        {
            deselectedColor = Color.grey;
            selectedColor = Color.yellow;
            normalLength = .25f;
            visibleInGame = true;   
            visibleInEditor = true;
        }
    }

    [SerializeField] private LineColliderAppearance appearance;

    private bool isSelected = false;

    public int NumPoints { get { return points.Count; } }
    public bool IsSelected { get { return isSelected; } set { isSelected = value; } }
    public Color DeselectedColor { get { return appearance.deselectedColor; } set { appearance.deselectedColor = value; } }
    public Color SelectedColor { get { return appearance.selectedColor; } set { appearance.selectedColor = value; } }

    public Vector2 GetPointWorldPosition(int index)
    {
        Vector3 localPosition = points[index].position;
        return transform.position + localPosition;
    }

    public void SetPointWorldPosition(int index, Vector3 worldPosition)
    {
        points[index].position = worldPosition - transform.position;
    }

    public Vector2 GetWorldNormal(int index)
    {
        return points[index].normal;
    }


    public bool IntersectLine(Vector3 lineStart, Vector3 lineEnd, out LineIntersectionResult intersectionResult)
    {
        bool result = false;
        intersectionResult = LineIntersectionResult.GetEmpty();

        float minDistance = float.MaxValue;
        for (int i = 0, j = 1; j < NumPoints; i++, j++)
        {
            Vector2 colliderStart = GetPointWorldPosition(i);
            Vector2 colliderEnd = GetPointWorldPosition(j);
            Vector2 normal = GetWorldNormal(i);

            Vector3 testIntersect = Vector3.zero;
            bool validIntersection = LineIntersections.IntersectLineLine(lineStart.x, lineEnd.x, colliderStart.x, colliderEnd.x, lineStart.y, lineEnd.y, colliderStart.y, colliderEnd.y, out testIntersect);

            if (validIntersection)
            {
                float testDistance = Vector2.Distance(lineStart, testIntersect);

                float dot = Vector2.Dot((lineEnd - lineStart).normalized, normal);

                if (testDistance < minDistance &&
                    dot < 0)
                {
                    result = true;
                    minDistance = testDistance;

                    float intersectDistance = Vector2.Distance(lineStart, testIntersect) / Vector2.Distance(lineStart, lineEnd);
                    intersectionResult.Init(testIntersect, normal, intersectDistance, true);
                }
            }
        }

        return result;
    }

    private void OnEnable()
    {
        LineCollisionScene.Instance.RegisterLineCollider(this);
    }

    protected void OnDisable()
    {
        // Do not access the singleton if the scene is being destroyed.
        if (gameObject.scene.isLoaded)
            LineCollisionScene.Instance.RemoveLineCollider(this);
    }

    protected void OnDrawGizmos()
    {
        if (!appearance.visibleInEditor && !appearance.visibleInGame) return;
        else if (SceneView.currentDrawingSceneView == null && !appearance.visibleInGame) return;
        else if (SceneView.currentDrawingSceneView != null && !appearance.visibleInEditor) return;

        Color displayColor = isSelected ? appearance.selectedColor : appearance.deselectedColor;

        for (int i = 0; i < NumPoints; i++)
        {
            Vector2 point = GetPointWorldPosition(i);

            Gizmos.color = displayColor;
            Gizmos.DrawIcon(point, "point", false, displayColor);
        }

        for (int p1 = 0, p2 = 1; p2 < NumPoints; p1++, p2++)
        {
            Vector2 lineStart = GetPointWorldPosition(p1);
            Vector2 lineEnd = GetPointWorldPosition(p2);
            Vector2 normal = GetWorldNormal(p1);

            Gizmos.color = displayColor;
            Gizmos.DrawLine(lineStart, lineEnd);

            Vector2 normalStart = (lineStart + lineEnd) / 2;

            Gizmos.color = Color.white;
            Gizmos.DrawLine(normalStart, normalStart + normal * appearance.normalLength);
        }
    }
}
