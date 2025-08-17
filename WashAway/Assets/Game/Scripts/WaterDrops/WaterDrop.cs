using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrop : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private float growRate = .02f;
    private float timeAlive = 0.0f;

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

        transform.localScale = transform.localScale + growRate * Time.deltaTime * Vector3.one;
        timeAlive += Time.deltaTime;
    }

    public void Reset()
    {
        transform.position = initialPosition;
        transform.localScale = initialScale;
        transform.rotation = initialRotation;
        timeAlive = 0.0f;
    }
}
