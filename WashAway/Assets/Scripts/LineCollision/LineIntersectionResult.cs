using UnityEngine;

public struct LineIntersectionResult
{
    public Vector3 intersectPosition;
    public Vector3 surfaceNormal;
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
        result.intersectPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, 0);
        result.surfaceNormal = Vector3.zero;
        result.intersectDistance = -1;
        result.validIntersection = false;
        return result;
    }
}
