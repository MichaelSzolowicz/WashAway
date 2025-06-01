using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollisionScene : MonoBehaviour
{
    private static LineCollisionScene instance;

    private List<ILineColliderInterface> lineColliders = new List<ILineColliderInterface>();

    public static LineCollisionScene Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<LineCollisionScene>();

                if (instance != null)
                {
                    return instance;
                }

                GameObject go = new GameObject();
                go.name = "LineCollisionScene";
                instance = go.AddComponent<LineCollisionScene>();
            }

            return instance;
        }
    }

    private void OnEnable()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void RegisterLineCollider(ILineColliderInterface lineColliderInterface)
    {
        if (lineColliders.Contains(lineColliderInterface))
            return;

        lineColliders.Add(lineColliderInterface);
    }

    public void RemoveLineCollider(ILineColliderInterface lineColliderInterface)
    {
        if (!lineColliders.Contains(lineColliderInterface))
            return;

        lineColliders.Remove(lineColliderInterface);
    }

    public bool IntersectLine(Vector3 lineStart, Vector3 lineEnd, out LineIntersectionResult lineIntersectionResult)
    {
        bool result = false;
        lineIntersectionResult = LineIntersectionResult.GetEmpty();

        if (lineStart == lineEnd)
        {
            return result;
        }

        float minDistance = float.PositiveInfinity;

        foreach (var lineCollider in lineColliders)
        {
            LineIntersectionResult testIntersect = LineIntersectionResult.GetEmpty();
            bool validIntersect = lineCollider.IntersectLine(lineStart, lineEnd, out testIntersect);

            if(validIntersect)
            {
                result = true;

                float testDistance = Vector2.Distance(lineStart, testIntersect.intersectPosition);

                if(testDistance < minDistance)
                {
                    minDistance = testDistance;
                    lineIntersectionResult = testIntersect;
                }
            }
        }

        return result;
    }

    public void ShowCount()
    {
        Debug.Log(lineColliders.Count);
    }
}
