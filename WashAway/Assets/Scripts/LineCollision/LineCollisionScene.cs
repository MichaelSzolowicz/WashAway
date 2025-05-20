using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollisionScene : MonoBehaviour
{
    private static LineCollisionScene _instance;

    public static LineCollisionScene Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<LineCollisionScene>();

                if (_instance != null)
                {
                    return _instance;
                }

                GameObject go = new GameObject();
                go.name = "LineCollisionScene";
                _instance = go.AddComponent<LineCollisionScene>();
            }

            return _instance;
        }
    }

    private void OnEnable()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        if (_instance != null)
        {
            _instance = null;
        }
    }

    private List<LineCollider> _lineColliders = new List<LineCollider>();

    public void RegisterLineCollider(LineCollider lineCollider)
    {
        if (_lineColliders.Contains(lineCollider))
            return;

        _lineColliders.Add(lineCollider);
    }

    public void RemoveLineCollider(LineCollider lineCollider)
    {
        if (!_lineColliders.Contains(lineCollider))
            return;

        _lineColliders.Remove(lineCollider);
    }

    public bool IntersectLine(Vector3 lineStart, Vector3 lineEnd, out LineIntersectionResult lineIntersectionResult)
    {
        bool result = false;
        lineIntersectionResult = LineIntersectionResult.GetEmpty();

        if (lineStart == lineEnd)
        {
            return false;
        }

        foreach (var lineCollider in _lineColliders)
        {
            LineIntersectionResult testIntersect = LineIntersectionResult.GetEmpty();
            bool validIntersect = lineCollider.IntersectLine(lineStart, lineEnd, out testIntersect);

            if(validIntersect)
            {
                result = true;

                if(Vector2.Distance(lineStart, testIntersect.intersectPosition) < Vector2.Distance(lineStart, lineIntersectionResult.intersectPosition))
                {
                    lineIntersectionResult = testIntersect;
                }
            }
        }

        return result;
    }

    public void ShowCount()
    {
        Debug.Log(_lineColliders.Count);
    }
}

