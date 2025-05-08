using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    protected const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;

    [SerializeField] protected float maxHorizontalSpeed;
    [SerializeField] protected float accelerationScale;
    [SerializeField] protected float gravityScale;

    protected Vector3 _velocity;

    void Update()
    {
        Move(Time.deltaTime);
    }

    protected void Move(float deltaTime)
    {
        _velocity += ACCELERATION_DUE_TO_GRAVITY * Vector3.down * gravityScale * deltaTime;

        transform.position += _velocity * deltaTime;
    }
}
