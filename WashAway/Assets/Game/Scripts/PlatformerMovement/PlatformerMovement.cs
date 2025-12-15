using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    private const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;
    private const float SMALL_NUMBER = .0015f;

    [Header("Walking")]
    [SerializeField] private float maxWalkSpeed = 10;
    [SerializeField] private float accelerationScale = 10;
    [SerializeField] private float gravityScale = 1;
    [SerializeField] private float brakingScale = 1;
    [SerializeField, Range(0f, 90f)] private float maxWalkableSlope;

    public float walkSpeed
    {
        get { return walkVelocity.magnitude; }
    }

    [Header("Jumping")]
    [SerializeField] private float jumpScale = 1;

    private Vector3 verticalVelocity;
    private Vector3 walkVelocity;

    private bool grounded;
    private LineIntersectionResult groundIntersection;
    public float probeDepth;

    private bool isFallingThrough = false;
    private Coroutine fallThroughCoroutine;

    private bool facingLeft = false;

    public bool FacingLeft { get { return facingLeft; } }

    public bool blockInput = false;

    // Test values
    [Header("Test")]
    public int targetFramerate = 30;
    public bool disableFallThrough = false;

    private void Start()
    {
        /* TESTONLY */
        Application.targetFrameRate = targetFramerate;
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

        // Only the Move() function is allowed to determine when we become grounded, this check determines if we are still grounded.
        if(grounded)
        {
            grounded = CheckGrounded();
        }

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

        //print("Velocity: " + (transform.position.x - previousPosition.x) / deltaTime);
        previousPosition = transform.position;
    }

    private Vector3 previousPosition = Vector3.zero;

    private bool CheckGrounded()
    {
        if (isFallingThrough)
        {
            return false;
        }

        Vector3 lineStart = transform.position; // + probeDepth * Vector3.up;
        Vector3 lineEnd = transform.position + probeDepth * Vector3.down;

        if (
            LineCollisionScene.Instance.IntersectLine(lineStart, lineEnd, out groundIntersection)
            && Vector2.Dot(groundIntersection.surfaceNormal, Vector3.down) <= 0
            //&& Mathf.Acos(Vector3.Dot(groundIntersection.surfaceNormal, Vector3.up)) * Mathf.Rad2Deg < maxWalkableSlope
            )
        {
            return true;
        }
        else
        {
            return false;
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
        if (disableFallThrough) return;

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
        if (blockInput) return Vector3.zero;

        Vector3 input = Vector3.zero;
        if (Input.GetKey(KeyCode.D))
        {
            input += Vector3.right;

            if (facingLeft)
            {
                TurnAround();
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            input += Vector3.left;

            if (!facingLeft)
            {
                TurnAround();
            }
        }

        return input;
    }

    public void TurnAround()
    {
        transform.Rotate(new Vector3(0, 180));
        facingLeft = !facingLeft;
    }

    private void Move(float deltaTime, Vector3 input)
    {
        Vector3 velocity = verticalVelocity;

        // Stick to slopes
        if (grounded)
        {
            Vector3 direction = Vector3.ProjectOnPlane(walkVelocity, groundIntersection.surfaceNormal).normalized;

            velocity += direction * walkVelocity.magnitude;
        }
        else
        {
            velocity += walkVelocity;
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

            if (validIntersection)
            {
                transform.position = testIntersection.intersectPosition - remainingMove.normalized * SMALL_NUMBER;

                float remainingDistance = remainingMove.magnitude * (1 - testIntersection.intersectDistance);
                remainingMove = Vector3.ProjectOnPlane(walkVelocity, testIntersection.surfaceNormal).normalized * remainingDistance;

                verticalVelocity = Vector3.zero;

                grounded = true;
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
