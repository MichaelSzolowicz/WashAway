using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class LineCollider : MonoBehaviour, ILineColliderInterface
{
    [SerializeField] private List<LinePoint> worldPoints = new List<LinePoint>();
    [HideInInspector] [SerializeField] private int numPoints = 0;

    [Header("Rendering")]
    [SerializeField] public Color defaultColor = Color.white;
    [SerializeField] public Color selectedColor = Color.yellow;
    [SerializeField] private float lineWidth = 1;
    [SerializeField] private float normalLength = .25f;
    [SerializeField] private bool visibleInGame = true;
    [SerializeField] private bool visibleInEditor = true;
    private bool isSelected = false;

    [HideInInspector] [SerializeField] private Vector3 previousPosition = Vector3.zero;

    public int NumPoints
    {
        get
        {
            return numPoints;
        }
    }

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;
        }
    }

    public LinePoint GetPoint(int index)
    {
        return worldPoints[index];
    }

    public void SetPointWorldPosition(int index, Vector3 worldPosition)
    {
        worldPoints[index].position = worldPosition;

        SetNormal(index);
        if (index - 1 >= 0)
            SetNormal(index - 1);
    }

    public bool IntersectLine(Vector3 lineStart, Vector3 lineEnd, out LineIntersectionResult intersectionResult)
    {
        bool result = false;
        intersectionResult = LineIntersectionResult.GetEmpty();

        float minDistance = float.MaxValue;
        for (int i = 0, j = 1; j < numPoints; i++, j++)
        {
            LinePoint colliderStart = worldPoints[i];
            LinePoint colliderEnd = worldPoints[j];

            Vector3 testIntersect = Vector3.zero;
            bool validIntersection = LineIntersections.IntersectLineLine(lineStart.x, lineEnd.x, colliderStart.x, colliderEnd.x, lineStart.y, lineEnd.y, colliderStart.y, colliderEnd.y, out testIntersect);

            if (validIntersection)
            {
                float testDistance = Vector2.Distance(lineStart, testIntersect);

                float dot = Vector2.Dot((lineEnd - lineStart).normalized, colliderStart.normal);

                if (testDistance < minDistance &&
                    dot < 0)
                {
                    result = true;
                    minDistance = testDistance;

                    float intersectDistance = Vector2.Distance(lineStart, testIntersect) / Vector2.Distance(lineStart, lineEnd);
                    intersectionResult.Init(testIntersect, colliderStart.normal, intersectDistance, true);
                }
            }
        }

        return result;
    }

    private void OnEnable()
    {
        if (Application.isPlaying)
            LineCollisionScene.Instance.RegisterLineCollider(this);
    }

    protected void OnDisable()
    {
        // Do not access the singleton if the scene is being destroyed.
        if (Application.isPlaying && gameObject.scene.isLoaded)
            LineCollisionScene.Instance.RemoveLineCollider(this);
    }

    protected void OnDrawGizmos()
    {
        if (!visibleInEditor && !visibleInGame) return;
        else if (SceneView.currentDrawingSceneView == null && !visibleInGame) return;
        else if(SceneView.currentDrawingSceneView != null && !visibleInEditor) return;

        Color useColor = isSelected ? selectedColor : defaultColor;
        Handles.color = useColor;
        for (int i = 0; i < numPoints; i++)
        {
            LinePoint point = GetPoint(i);

            Gizmos.DrawIcon(point.position, "point", false, useColor);
        }

        for (int p1 = 0, p2 = 1; p2 < numPoints; p1++, p2++)
        {
            LinePoint lineStart = GetPoint(p1);
            LinePoint lineEnd = GetPoint(p2);

            Handles.color = useColor;
            Handles.DrawLine(lineStart.position, lineEnd.position, lineWidth);

            Handles.color = Color.white;
            Vector2 normalStart = (lineStart.position + lineEnd.position) / 2;
            Handles.DrawLine(normalStart, normalStart + worldPoints[p1].normal * normalLength);
        }
    }

    protected void OnValidate()
    {
        if (worldPoints.Count != numPoints)
            InitializePoint();
        else
            SanitizePoints();
    }

    protected void InitializePoint()
    {
        if (numPoints == 0 && worldPoints.Count > 0)
        {
            worldPoints[0].position = transform.position;
        }

        for (int p0 = numPoints - 2, p1 = numPoints - 1, p2 = numPoints; p2 < worldPoints.Count; p0++, p1++, p2++)
        {
            if(p1 < 0) continue;

            Vector2 deltaPosition = Vector2.zero;

            if (p0 >= 0)
            {
                deltaPosition = (worldPoints[p1].position - worldPoints[p0].position).normalized;
            }

            if (deltaPosition.magnitude <= .1f)
                deltaPosition = Vector2.right;

            worldPoints[p2].position = worldPoints[p1].position + deltaPosition;
            SetNormal(p1);
        }

        numPoints = worldPoints.Count;
    }

    private void SanitizePoints()
    {
        for(int i = 0; i < numPoints; i++)
        {
            SetNormal(i);  
        }
    }

    protected void SetNormal(int index)
    {
        LinePoint p1 = worldPoints[index];

        if (index + 1 < worldPoints.Count)
        {
            LinePoint p2 = worldPoints[index + 1];

            p1.normal = Quaternion.Euler(0, 0, 90) * (p2.position - p1.position).normalized;
        }
        else
        {
            p1.normal = Vector3.up;
        }

        if (Vector2.Dot(p1.normal, Vector2.up) < 0)
        {
            p1.normal = -1 * p1.normal;
        }
    }

    public void Update()
    {
        if (previousPosition != transform.position)
        {
            Vector2 deltaPosition = transform.position - previousPosition;
            for (int i = 0; i < numPoints; i++)
            {
                worldPoints[i].position = worldPoints[i].position + deltaPosition;
            }
        }

        previousPosition = transform.position;
    }
}

