using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;
    private const float SMALL_NUMBER = .0015f;

    [Header("Walking")]
    [SerializeField] private float maxWalkSpeed = 10;
    [SerializeField] private float accelerationScale = 10;
    [SerializeField] private float gravityScale = 1;
    [SerializeField] private float brakingScale = 1;
    [SerializeField, Range(0f, 90f)] private float maxWalkableSlope;

    [Header("Jumping")]
    [SerializeField] private float jumpScale = 1;

    private Vector3 verticalVelocity;
    private Vector3 walkVelocity;

    private bool grounded;
    private LineIntersectionResult groundIntersection;
    public float probeDepth;

    private bool isFallingThrough = false;
    private Coroutine fallThroughCoroutine;

    private void Start()
    {
        /* TESTONLY */
        //Application.targetFrameRate = 30;
        /* ENDTEST */
    }


    private void Update()
    {
        if (
            !GameState.Paused
            && !GameState.CurrentLevelClear
            )
        {
            UpdatePhysicsState(Time.deltaTime);
        }
    }

    private void UpdatePhysicsState(float deltaTime)
    {
        // Gravity
        verticalVelocity += ACCELERATION_DUE_TO_GRAVITY * Vector3.down * gravityScale * deltaTime;

        CheckGrounded();

        CheckJumping();

        CheckFallThrough();

        // Raw input
        Vector3 input = GetInput();
        Vector3 inputCopy = input;

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

        Move(deltaTime, inputCopy);
    }

    private void CheckGrounded()
    {
        if(isFallingThrough)
        {
            grounded = false;
            return;
        }

        Vector3 lineStart = transform.position; // + probeDepth * Vector3.up;
        Vector3 lineEnd = transform.position + probeDepth * Vector3.down;

        if(
            LineCollisionScene.Instance.IntersectLine(lineStart, lineEnd, out groundIntersection)
            && Vector2.Dot(groundIntersection.surfaceNormal, Vector3.down) <= 0
            //&& Mathf.Acos(Vector3.Dot(groundIntersection.surfaceNormal, Vector3.up)) * Mathf.Rad2Deg < maxWalkableSlope
            )
        {
            grounded = true;
        }
        else
        {
            grounded= false;
        }
    }
    
    private void CheckJumping()
    {
        if (
            Input.GetKeyDown(KeyCode.Space) 
            && grounded
            )
        {

            verticalVelocity = jumpScale * Vector3.up;
        }
    }

    private void CheckFallThrough()
    {
        if (
            Input.GetKeyDown(KeyCode.S)
            && grounded
            )
        {

            isFallingThrough = true;
        }
    }

    private Vector2 GetInput()
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

    private void Move(float deltaTime, Vector3 input)
    {
        Debug.DrawLine(transform.position, transform.position + walkVelocity.normalized, Color.blue, .1f);

        Vector3 velocity = verticalVelocity;

        // Stick to slopes
        if (grounded)
        {
            Vector3 direction = Vector3.ProjectOnPlane(walkVelocity, groundIntersection.surfaceNormal).normalized;

            velocity = direction * walkVelocity.magnitude;
            velocity += verticalVelocity;
        }
        else
        {
            velocity = walkVelocity + verticalVelocity;
        }

        Vector3 remainingMove = velocity * deltaTime;

        // Move
        int maxIterations = 3;
        for (int iterations = 0; iterations < maxIterations && remainingMove.magnitude > 0; iterations++)
        {
            Vector3 lineStart = transform.position;
            Vector3 lineEnd = lineStart + remainingMove;

            LineIntersectionResult testIntersection = LineIntersectionResult.GetEmpty();
            bool validIntersection = LineCollisionScene.Instance.IntersectLine(lineStart, lineEnd, out testIntersection);

            // Ideally, if falling through we would do a multi intersect and discard only the nearest intersection.
            // I am too lazy to implement multi-intersection right now so instead you will fall through multiple platforms if they are very close.
            if (isFallingThrough && validIntersection)
            {
                validIntersection = false;

                isFallingThrough = false;
            }

            if (validIntersection &&
                Vector2.Dot(testIntersection.surfaceNormal, remainingMove.normalized) <= 0)
            {
                transform.position = testIntersection.intersectPosition - remainingMove.normalized * SMALL_NUMBER;

                float remainingDistance = remainingMove.magnitude * (1 - testIntersection.intersectDistance);
                remainingMove = Vector3.ProjectOnPlane(walkVelocity, testIntersection.surfaceNormal).normalized * remainingDistance;
                    
                verticalVelocity = Vector3.zero;
            }
            else
            {
                transform.position += remainingMove;

                remainingMove = Vector3.zero;
            }
        }

    }

    public void ResetPhysicsState()
    {
        isFallingThrough = false;
        walkVelocity = Vector3.zero;
        verticalVelocity = Vector3.zero;
    }

}
