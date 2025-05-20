using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILineColliderInterface
{
    public bool IntersectLine(Vector3 lineStart, Vector3 lineEnd, out LineIntersectionResult lineIntersectionResult);
}
