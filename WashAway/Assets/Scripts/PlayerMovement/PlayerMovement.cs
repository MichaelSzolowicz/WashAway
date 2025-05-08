using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;

    [SerializeField] protected float maxHorizontalSpeed = 10;
    [SerializeField] protected float accelerationScale = 10;
    [SerializeField] protected float gravityScale = 1;

    protected Vector3 _velocity;

    void Update()
    {
        Move(Time.deltaTime);
    }

    protected void Move(float deltaTime)
    {
        _velocity += ACCELERATION_DUE_TO_GRAVITY * Vector3.down * gravityScale * deltaTime;

        if (Input.GetKey(KeyCode.D))
        {
            _velocity += accelerationScale * Vector3.right * deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _velocity += accelerationScale * Vector3.left * deltaTime;
        }

        Vector2 horizontalVelocity = _velocity;
        if (horizontalVelocity.magnitude > maxHorizontalSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxHorizontalSpeed;
            _velocity = new Vector3(horizontalVelocity.x, horizontalVelocity.y, _velocity.z);
        }

        transform.position += _velocity * deltaTime;
    }
}
