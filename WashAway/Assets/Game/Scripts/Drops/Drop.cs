using System.Collections;
using UnityEngine;

public class Drop : MonoBehaviour
{
    private enum State
    {
        SpawnDelay,
        Growing,
        Falling
    }

    private const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;

    // state
    private State state = State.SpawnDelay;
    private float timeInCurrentState = 0.0f;

    // animation
    private float spawnDelay = 3.0f;
    private float growTime = 1;
    private float gravityScale = 1.2f;
    private float growRate = .12f;
    private Vector3 velocity = Vector3.zero;

    private float timeAlive = 0.0f;
    public float TimeAlive { get { return timeAlive; } }

    private void OnEnable()
    {
        ResetAnimation();
    }

    public void Animate(float deltaTime)
    {
        if(!enabled) return;

        switch (state)
        {
            case State.SpawnDelay:
                SpawnDelay();
                break;
            case State.Growing:
                Growing();
                break;
            case State.Falling:
                Falling();
                break;
            default:
                break;
        }

        timeAlive += deltaTime;
    }

    private void SpawnDelay()
    {
        if (timeInCurrentState < spawnDelay)
        {
            timeInCurrentState += Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(true);
            timeInCurrentState = 0.0f;
            state = State.Growing;
        }
    }

    private void Growing()
    {
        transform.localScale = transform.localScale + growRate * Time.deltaTime * Vector3.one;
        timeInCurrentState += Time.deltaTime;

        if (timeInCurrentState >= growTime)
        {
            state = State.Falling;
            timeInCurrentState = 0.0f;
        }
    }
    
    private void Falling()
    {
        velocity += gravityScale * ACCELERATION_DUE_TO_GRAVITY * Time.deltaTime * Vector3.down;
        transform.position = transform.position + velocity * Time.deltaTime;

        timeInCurrentState += Time.deltaTime;
    }

    public void ResetAnimation()
    {
        velocity = Vector3.zero;
        timeAlive = 0.0f;
        timeInCurrentState = 0.0f;
        state = State.SpawnDelay;
    }
}
