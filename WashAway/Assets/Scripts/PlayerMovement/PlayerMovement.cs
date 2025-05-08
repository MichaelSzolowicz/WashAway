using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;

    [SerializeField] protected float maxHorizontalSpeed = 10;
    [SerializeField] protected float accelerationScale = 10;
    [SerializeField] protected float gravityScale = 1;
    [SerializeField] protected float brakingScale = 1;

    protected Vector3 _velocity;

    void Update()
    {
        Move(Time.deltaTime);
    }

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

        float horizontalVelocity = _velocity.x;
        if (horizontalVelocity > maxHorizontalSpeed)
        {
            horizontalVelocity = horizontalVelocity * maxHorizontalSpeed;
        }

        if (input.magnitude <= 0)
        {
            float braking = brakingScale * horizontalVelocity * deltaTime;
            if (braking > horizontalVelocity)
            {
                horizontalVelocity = 0;
            }
            else
            {
                horizontalVelocity -= braking;
            }
        }

        _velocity.x = horizontalVelocity;

        transform.position += _velocity * deltaTime;
    }
}
