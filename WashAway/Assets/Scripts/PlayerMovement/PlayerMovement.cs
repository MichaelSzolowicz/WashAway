using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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

    /* TESTONLY */
    private void Start()
    {
        Application.targetFrameRate = 30;
    }
    /* ENDTEST */

    private void Update()
    {
        UpdatePhysicsState(Time.deltaTime);
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

    private void CheckGrounded()
    {
        Vector3 lineStart = transform.position + probeDepth * Vector3.up;
        Vector3 lineEnd = transform.position + probeDepth * Vector3.down;

        //print(lineStart + ", " + lineEnd);
        //Debug.DrawLine(lineStart, lineEnd, Color.red, 1);

        grounded = LineCollisionScene.Instance.IntersectLine(lineStart, lineEnd, out groundIntersection);

        //print(grounded);
    }

    private void CheckJumping()
    {
        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            verticalVelocity += jumpScale * Vector3.up;
        }
    }

    private void CheckFallThrough()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            StartFallThrough();
        }
        
        if(Input.GetKeyUp(KeyCode.S))
        {
            StopFallThrough();
        }
    }

    private void StartFallThrough()
    {
        StopFallThrough();  

        fallThroughCoroutine = StartCoroutine("FallThrough");
    }

    private void StopFallThrough()
    {
        if(fallThroughCoroutine != null)
        {
            StopCoroutine(fallThroughCoroutine);
            fallThroughCoroutine = null;
        }

        isFallingThrough = false;
    }

    private IEnumerator FallThrough()
    {
        isFallingThrough = true;

        yield return new WaitForEndOfFrame();

        isFallingThrough = false;
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

    private void Move(float deltaTime)
    {
        Vector3 velocity = verticalVelocity + walkVelocity;

        Vector3 remainingMove = velocity * deltaTime;

        // Move
        int maxIterations = 3;
        for (int iterations = 0; iterations < maxIterations && remainingMove.magnitude > 0; iterations++)
        {
            Vector3 lineStart = transform.position;
            Vector3 lineEnd = lineStart + remainingMove;

            LineIntersectionResult testIntersection = LineIntersectionResult.GetEmpty();
            bool validItersection = LineCollisionScene.Instance.IntersectLine(lineStart, lineEnd, out testIntersection);

            // Ideally, if falling through we would do a multi intersect and discard only the nearest intersection.
            // I am too lazy to implement multi-intersection right now so instead you will fall through multiple platforms if they are very close.
            validItersection = validItersection && !isFallingThrough;
            StopFallThrough();

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
}
