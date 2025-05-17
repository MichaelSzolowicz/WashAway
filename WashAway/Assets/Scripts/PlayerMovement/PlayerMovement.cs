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

    protected LineIntersectionResult intersectionResult = new LineIntersectionResult();
    protected bool grounded = false;

    void Update()
    {
        Move(Time.deltaTime);
    }

    protected void Move(float deltaTime)
    {
        LineIntersectionResult groundIntersection = LineIntersectionResult.GetEmpty();
        if (grounded)
        {
            grounded = CheckGrounded(out groundIntersection);
        }

        if(!grounded)
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

        if(input.magnitude == 0)
        {
            walkSpeed -= brakingScale * Time.deltaTime;
            if(walkSpeed < 0)
                walkSpeed = 0;
        }

        if(grounded)
        {
            _walkVelocity = Vector3.ProjectOnPlane(_walkVelocity.normalized, groundIntersection.surfaceNormal).normalized * walkSpeed;
        }
        else
        {
            _walkVelocity = _walkVelocity.normalized * walkSpeed;
        }

        Vector3 velocity = _walkVelocity + _verticalVelocity * Vector2.up;

        Debug.DrawLine(transform.position, transform.position + velocity.normalized, Color.magenta);

        bool foundIntersect = LineCollisionScene.Instance.IntersectLine(transform.position, transform.position + velocity * deltaTime, out intersectionResult);

        if (foundIntersect)
        {
            if (Vector2.Dot(velocity.normalized, intersectionResult.surfaceNormal) > 0)
            { }
            else if (Vector2.Angle(Vector3.up, intersectionResult.surfaceNormal) > maxWalkableSlope) { }
            else
            {
                grounded = true;
                transform.position = intersectionResult.intersectPosition;
                _verticalVelocity = 0;
                return;
            }
        }

        transform.position += velocity * deltaTime;
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

    protected bool CheckGrounded(out LineIntersectionResult intersectionResult)
    {
        bool validIntersection = LineCollisionScene.Instance.IntersectLine(transform.position + Vector3.up * .1f, transform.position + Vector3.down * .1f, out intersectionResult);

        if (!validIntersection) return false;

        print(Vector2.Angle(Vector3.up, intersectionResult.surfaceNormal));

        if (Vector2.Angle(Vector3.up, intersectionResult.surfaceNormal) > maxWalkableSlope) return false;

        //if (Vector2.Dot((_walkVelocity + _verticalVelocity * Vector2.up).normalized, intersectionResult.surfaceNormal) > 0) return false;

         return true;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (grounded)
            Gizmos.DrawSphere(transform.position, .1f);

        if (!intersectionResult.validIntersection) return;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(intersectionResult.intersectPosition, intersectionResult.intersectPosition + intersectionResult.surfaceNormal);
    }
}
