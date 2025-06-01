using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;
    private const float SMALL_NUMBER = .0015f;

    [Header("Movement")]
    [SerializeField] private float maxWalkSpeed = 10;
    [SerializeField] private float accelerationScale = 10;
    [SerializeField] private float gravityScale = 1;
    [SerializeField] private float brakingScale = 1;
    [SerializeField, Range(0f, 90f)] private float maxWalkableSlope;

    private Vector3 verticalVelocity;
    private Vector3 walkVelocity;

    public Vector3 Velocity
    {
        get {  return verticalVelocity + walkVelocity; }
    }

    /* TESTONLY */
    protected void Start()
    {
        Application.targetFrameRate = 30;
    }
    /* ENDTEST */

    void Update()
    {
        UpdatePhysicsState(Time.deltaTime);
    }

    private void UpdatePhysicsState(float deltaTime)
    {
        // Gravity
        verticalVelocity += ACCELERATION_DUE_TO_GRAVITY * Vector3.down * gravityScale * deltaTime;

        // Raw input
        Vector3 input = GetInput();

        walkVelocity += input * accelerationScale * deltaTime;

        // Max speed
        float walkSpeed = walkVelocity.magnitude;

        if (walkSpeed > maxWalkSpeed)
        {
            walkSpeed = maxWalkSpeed;
        }

        // Braking
        if (input.magnitude <= SMALL_NUMBER)
        {
            walkSpeed -= brakingScale * deltaTime;
            if (walkSpeed < 0)
            {
                walkSpeed = 0;
            }
        }

        // Finalize movement
        walkVelocity = walkVelocity.normalized * walkSpeed;

        Move(deltaTime);
    }

    protected void Move(float deltaTime)
    {
        Vector3 remainingMove = Velocity * deltaTime;

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

                transform.position = testIntersection.intersectPosition - remainingMove.normalized * SMALL_NUMBER;

                float remainingDistance = remainingMove.magnitude * (1 - testIntersection.intersectDistance);
                remainingMove = Vector3.ProjectOnPlane(remainingMove, testIntersection.surfaceNormal).normalized * remainingDistance;

                verticalVelocity = Vector3.zero;
            }
            else
            {
                transform.position += remainingMove;

                remainingMove = Vector3.zero;
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
}
