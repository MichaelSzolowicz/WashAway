using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;
    protected const float SMALL_NUMBER = .001f;

    [SerializeField] protected float maxWalkSpeed = 10;
    [SerializeField] protected float accelerationScale = 10;
    [SerializeField] protected float gravityScale = 1;
    [SerializeField] protected float brakingScale = 1;

    protected Vector3 _velocity;

    void Update()
    {
        Move(Time.deltaTime);
    }

    LineIntersectionResult intersectionResult = new LineIntersectionResult();
    protected void Move(float deltaTime)
    {
        _velocity += ACCELERATION_DUE_TO_GRAVITY * Vector3.down * gravityScale * deltaTime;

        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.D))
        {
            input += Vector3.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            input += Vector3.left;
        }

        _velocity += input * accelerationScale * deltaTime;

        float horizontalSpeed = Mathf.Abs(_velocity.x);
        float horizontalDirection = horizontalSpeed <= SMALL_NUMBER ? input.x : horizontalSpeed / _velocity.x;

        if (horizontalSpeed > maxWalkSpeed)
        {
            horizontalSpeed = maxWalkSpeed;
        }

        if (input.magnitude <= 0)
        {
            float braking = brakingScale * deltaTime;
            if (braking > horizontalSpeed)
            {
                horizontalSpeed = 0;
            }
            else
            {
                horizontalSpeed -= braking;
            }
        }

        _velocity.x = horizontalSpeed * horizontalDirection;

        bool foundIntersect = LineCollisionScene.Instance.IntersectLine(transform.position - _velocity.normalized * SMALL_NUMBER, transform.position + _velocity * deltaTime, out intersectionResult);

        if (foundIntersect)
        {
            transform.position = intersectionResult.intersectPosition;
            _velocity = Vector3.zero;
        }
        else
        {
            transform.position += _velocity * deltaTime;
        }
    }

    protected void CollisionTest(Vector3 deltaPos)
    {

    }

    public void OnDrawGizmos()
    {
        if (!intersectionResult.validIntersection) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(intersectionResult.intersectPosition, .1f);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(intersectionResult.intersectPosition, intersectionResult.intersectPosition + intersectionResult.surfaceNormal);
    }
}
