using UnityEngine;

public struct LineIntersectionResult
{
    public Vector2 intersectPosition;
    public Vector2 surfaceNormal;
    public float intersectDistance;
    public bool validIntersection;

    public static LineIntersectionResult Get(Vector2 intersectPosition, Vector2 surfaceNormal, float intersectDistance, bool validIntersection = false)
    {
        LineIntersectionResult result = new LineIntersectionResult();
        result.intersectPosition = intersectPosition;
        result.surfaceNormal = surfaceNormal;
        result.intersectDistance = intersectDistance;
        result.validIntersection = validIntersection;
        return result;
    }

    public static LineIntersectionResult GetEmpty()
    {
        LineIntersectionResult result = new LineIntersectionResult();
        result.intersectPosition = Vector3.positiveInfinity;
        result.surfaceNormal = Vector2.zero;
        result.intersectDistance = -1;
        result.validIntersection = false;
        return result;
    }
}
