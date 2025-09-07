using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    private const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;

    private float timeAlive = 0.0f;

    private float growTime = 1.1f;
    private float gravityScale = .1f;
    private float growRate = .02f;
    private Vector3 velocity = Vector3.zero;

    private bool paused = false;
    public bool Paused { get { return paused; } set { paused = value; } }

    public float TimeAlive
    {
        get {  return timeAlive; }
    }

    private void Update()
    {
        if (paused) return;

        if(timeAlive < growTime)
        {
            transform.localScale = transform.localScale + growRate * Time.deltaTime * Vector3.one;
        }
        else
        {
            velocity += gravityScale * ACCELERATION_DUE_TO_GRAVITY * Time.deltaTime * Vector3.down;
            transform.position += velocity * Time.deltaTime;
        }

        timeAlive += Time.deltaTime;
    }

    public void RestartMovement()
    {
        velocity = Vector3.zero;
        timeAlive = 0.0f;
    }
}
