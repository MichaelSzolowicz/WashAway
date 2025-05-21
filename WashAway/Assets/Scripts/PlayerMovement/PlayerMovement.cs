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
    [SerializeField, Range(0f, 90f)] protected float maxWalkableSlope;

    protected float _verticalVelocity;
    protected Vector2 _walkVelocity;

    protected void Start()
    {
        Application.targetFrameRate = 0;
    }

    void Update()
    {
        Move2(Time.deltaTime);
    }

    protected LineIntersectionResult intersection = LineIntersectionResult.GetEmpty();

    protected void Move2(float deltaTime)
    {
        if(!intersection.validIntersection) 
            _verticalVelocity -= ACCELERATION_DUE_TO_GRAVITY * gravityScale * deltaTime;
        else
            _verticalVelocity = 0;

        Vector2 input = GetInput();

        _walkVelocity += input * accelerationScale * deltaTime;

        float walkSpeed = _walkVelocity.magnitude;

        if (walkSpeed > maxWalkSpeed)
        {
            walkSpeed = maxWalkSpeed;
        }

        if(input.magnitude <= .001f)
        {
            walkSpeed -= brakingScale * deltaTime;
            if(walkSpeed < 0)
            {
                walkSpeed = 0;
            }

            _walkVelocity = _walkVelocity.normalized * walkSpeed;
        }

        Vector2 velocity = _walkVelocity + _verticalVelocity * Vector2.up;

        Vector2 remainingMove = velocity * deltaTime;

        //print(remainingMove);

        int maxIterations = 5;
        for (int iterations = 0; iterations < maxIterations && remainingMove.magnitude > .001f; iterations++)
        {
            Vector2 lineStart = transform.position;
            Vector2 lineEnd = lineStart + remainingMove;

            if(intersection.validIntersection)
            {
                lineStart = lineStart + intersection.surfaceNormal * .01f;
                lineEnd = lineEnd - intersection.surfaceNormal * .01f + remainingMove.normalized * .01f;
            }

            bool validItersection = LineCollisionScene.Instance.IntersectLine(lineStart, lineEnd, out intersection);

            //if(validItersection)
                print(intersection.surfaceNormal);

            if (validItersection &&
                Vector2.Dot(intersection.surfaceNormal, remainingMove.normalized) <= 0)
            {
                transform.position = intersection.intersectPosition;

                float remainingDistance = remainingMove.magnitude * (1 - intersection.intersectDistance);

                Vector2 projection = Vector3.ProjectOnPlane(remainingMove, intersection.surfaceNormal).normalized * remainingDistance;

                remainingMove = projection;

                _verticalVelocity = 0;
            }
            else
            {
                Vector3 remainder = remainingMove;

                transform.position += remainder;

                remainingMove = Vector2.zero;
            }
        }
    }

    protected Vector2 GetInput()
    {
        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.D))
        {
            input += Vector3.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            input += Vector3.left;
        }

        return input;
    }

    /*
    protected bool CheckGrounded()
    {
        bool validIntersection = LineCollisionScene.Instance.IntersectLine(transform.position, transform.position + Vector3.down * .01f, out groundIntersection);

        if (!validIntersection) return false;

        //print(Vector2.Angle(Vector3.up, intersectionResult.surfaceNormal));

        if (Vector2.Angle(Vector3.up, groundIntersection.surfaceNormal) > maxWalkableSlope) return false;

        //if (Vector2.Dot((_walkVelocity + _verticalVelocity * Vector2.up).normalized, intersectionResult.surfaceNormal) > 0) return false;

         return true;
    }
    */

    /*
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (grounded)
            Gizmos.DrawSphere(transform.position, .1f);

        if (!groundIntersection.validIntersection) return;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(groundIntersection.intersectPosition, groundIntersection.intersectPosition + groundIntersection.surfaceNormal);
    }
    */
}
