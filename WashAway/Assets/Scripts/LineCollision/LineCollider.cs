using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class LineCollider : MonoBehaviour
{
    [SerializeField] protected List<LinePoint> _points = new List<LinePoint>();

    [SerializeField] protected float rotation = 0;

    [SerializeField] protected float width = 0;

    [SerializeField] protected bool visibleInGame = true;
    [SerializeField] protected bool visibleInEditor = true;

    [SerializeField] protected Color defaultColor = Color.white;
    [SerializeField] protected Color selectedColor = Color.yellow;
    [SerializeField] protected float pointSize = .01f;
    [SerializeField] protected float lineWidth = 1;

    protected bool isSelected = false;

    public int Length
    {
        get
        {
            return _points.Count;
        }
    }

    public float Rotation
    {
        get
        {
            return rotation;
        }
    }

    public float Width
    {
        get
        {
            return width;
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

    private void OnEnable()
    {
        if(Application.isPlaying)
            LineCollisionScene.Instance.RegisterLineCollider(this);
    }

    public Vector3 GetPointWorldSpace(int index)
    {
        if(index < 0 || index >= _points.Count)
        {
            throw new IndexOutOfRangeException("Index out of range.");
        }

        Vector3 rotatedPoint = new Vector3();

        float rotation = Mathf.Deg2Rad * this.rotation;
        rotatedPoint.x = _points[index].x * Mathf.Cos(rotation) - _points[index].y * Mathf.Sin(rotation);
        rotatedPoint.y = _points[index].x * Mathf.Sin(rotation) + _points[index].y * Mathf.Cos(rotation);

        return transform.position + rotatedPoint;
    }

    public void SetPointWorldPosition(int index, Vector3 worldPosition)
    {
        if (index < 0 || index >= _points.Count)
        {
            throw new IndexOutOfRangeException("Index out of range.");
        }

        Vector3 localPosition = worldPosition - transform.position;

        Vector3 rotatedPoint = new Vector3();

        float rotation = Mathf.Deg2Rad * this.rotation;
        rotatedPoint.x = (localPosition.x * Mathf.Cos(-rotation) - localPosition.y * Mathf.Sin(-rotation));
        rotatedPoint.y = (localPosition.y * Mathf.Cos(-rotation) + localPosition.x * Mathf.Sin(-rotation));

        _points[index].Position = rotatedPoint;
    }

    protected void OnDisable()
    {
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
        for (int i = 0; i < Length; i++)
        {
            Vector3 position = GetPointWorldSpace(i);

            Gizmos.DrawIcon(position, "point", false, useColor);
        }

        for (int p1 = 0, p2 = 1; p2 < Length; p1++, p2++)
        {
            Handles.DrawLine(GetPointWorldSpace(p1), GetPointWorldSpace(p2), lineWidth);
        }
    }

    /*
    public void Update()
    {
        if(Application.isPlaying)
            LineCollisionScene.Instance.ShowCount();
    }
    */
}

