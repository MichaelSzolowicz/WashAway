using UnityEngine;

public struct LineIntersectionResult
{
    public Vector3 intersectPosition;
    public Vector3 surfaceNormal;
    public float intersectDistance;
    public bool validIntersection;

    public void Init(Vector2 intersectPosition, Vector2 surfaceNormal, float intersectDistance, bool validIntersection = false)
    {
        this.intersectPosition = intersectPosition;
        this.surfaceNormal = surfaceNormal;
        this.intersectDistance = intersectDistance;
        this.validIntersection = validIntersection;
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
