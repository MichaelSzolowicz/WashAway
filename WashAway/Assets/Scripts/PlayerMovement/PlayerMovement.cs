using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;
    protected const float SMALL_NUMBER = .01f;

    [SerializeField] protected float maxWalkSpeed = 10;
    [SerializeField] protected float accelerationScale = 10;
    [SerializeField] protected float gravityScale = 1;
    [SerializeField] protected float brakingScale = 1;
    [SerializeField, Range(0f, 90f)] protected float maxWalkableSlope;

    protected Vector3 _verticalVelocity;
    protected Vector3 _walkVelocity;

    protected void Start()
    {
        Application.targetFrameRate = 0;
    }

    void Update()
    {
        Move2(Time.deltaTime);
    }

    protected void Move2(float deltaTime)
    {
        // Gravity
        _verticalVelocity += ACCELERATION_DUE_TO_GRAVITY * Vector3.down * gravityScale * deltaTime;

        // Raw input
        Vector3 input = GetInput();

        _walkVelocity += input * accelerationScale * deltaTime;

        // Max speed
        float walkSpeed = _walkVelocity.magnitude;

        if (walkSpeed > maxWalkSpeed)
        {
            walkSpeed = maxWalkSpeed;
        }

        // Braking
        if(input.magnitude <= .001f)
        {
            walkSpeed -= brakingScale * deltaTime;
            if(walkSpeed < 0)
            {
                walkSpeed = 0;
            }
        }

        // Finalize movement
        _walkVelocity = _walkVelocity.normalized * walkSpeed;

        Vector3 velocity = _walkVelocity + _verticalVelocity;

        Vector3 remainingMove = velocity * deltaTime;

        // Move
        int maxIterations = 3;
        for (int iterations = 0; iterations < maxIterations && remainingMove.magnitude > 0; iterations++)
        {
            Vector3 lineStart = transform.position;
            Vector3 lineEnd = lineStart + remainingMove;

            LineIntersectionResult testIntersection = LineIntersectionResult.GetEmpty();
            bool validItersection = LineCollisionScene.Instance.IntersectLine(lineStart, lineEnd, out testIntersection);

            if (validItersection &&
                Vector2.Dot(testIntersection.surfaceNormal, remainingMove.normalized) <= 0)
            {
                /*
                Color[] colors = { Color.red, Color.cyan, Color.green, Color.blue, Color.gray };
                Vector3 remainingMove3 = remainingMove;
                Debug.DrawLine(transform.position, transform.position + remainingMove3, colors[iterations]);
                Debug.DrawLine(testIntersection.intersectPosition, testIntersection.intersectPosition + testIntersection.surfaceNormal * .01f, colors[iterations]);
                print(remainingMove);
                */

                //Vector2 backwardProject = remainingMove.magnitude > .01f ? remainingMove.normalized * .01f : 
                //print(testIntersection.intersectPosition);
                transform.position = testIntersection.intersectPosition - remainingMove.normalized * .01f;

                float remainingDistance = remainingMove.magnitude * (1 - testIntersection.intersectDistance);
                Vector2 projection = Vector3.ProjectOnPlane(remainingMove, testIntersection.surfaceNormal).normalized * remainingDistance;
                remainingMove = projection;

                _verticalVelocity = Vector3.zero;
            }
            else
            {
                transform.Translate(remainingMove);

                remainingMove = Vector3.zero;
            }
        }
    }

    //LineIntersectionResult testIntersection;

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
