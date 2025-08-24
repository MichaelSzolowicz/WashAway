using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    private const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;

    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private float timeAlive = 0.0f;

    private float growTime = 1.1f;
    private float gravityScale = .1f;
    private float growRate = .02f;
    private Vector3 velocity = Vector3.zero;

    public float TimeAlive
    {
        get {  return timeAlive; }
    }

    public void Init()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (GameState.Paused) return;
        if (GameState.CurrentLevelClear) return;

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

    public void Reset()
    {
        velocity = Vector3.zero;
        transform.position = initialPosition;
        transform.localScale = initialScale;
        transform.rotation = initialRotation;
        timeAlive = 0.0f;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
    }
}
