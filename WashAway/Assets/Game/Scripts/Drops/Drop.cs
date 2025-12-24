using System.Collections;
using UnityEngine;

public class Drop
{
    private enum State
    {
        SpawnDelay,
        Growing,
        Falling
    }

    private const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;

    // config
    private PaintedSprite sprite;
    private float textureTilesPerMeter;

    // state
    private bool enabled = false;
    private State state = State.SpawnDelay;
    private float timeInCurrentState = 0.0f;

    // animation
    private float spawnDelay = 3.0f;
    private float growTime = 1;
    private float gravityScale = 1.2f;
    private float growRate = .12f;
    private Vector2 velocity = Vector3.zero;
    private Vector2 position;
    private Vector2 scale;

    private float timeAlive = 0.0f;
    public float TimeAlive { get { return timeAlive; } }

    public Drop(PaintedSprite sprite, float textureTilesPerMeter)
    {
        this.sprite = sprite;
        sprite.enabled = false;
        enabled = true;
        state = State.SpawnDelay;
        this.textureTilesPerMeter = textureTilesPerMeter;
    }

    public void Update(float deltaTime)
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

        timeAlive += Time.deltaTime;
        PaintedSpriteConstraint.ConstrainSpriteToWorldPositionScale(position, scale, textureTilesPerMeter, out sprite.offset, out sprite.scale);
    }

    private void SpawnDelay()
    {
        if (timeInCurrentState < spawnDelay)
        {
            timeInCurrentState += Time.deltaTime;
        }
        else
        {
            sprite.enabled = true;
            timeInCurrentState = 0.0f;
            state = State.Growing;
        }
    }

    private void Growing()
    {
        scale = scale + growRate * Time.deltaTime * Vector2.one;
        timeInCurrentState += Time.deltaTime;

        if (timeInCurrentState >= growTime)
        {
            state = State.Falling;
            timeInCurrentState = 0.0f;
        }
    }
    
    private void Falling()
    {
        velocity += gravityScale * ACCELERATION_DUE_TO_GRAVITY * Time.deltaTime * Vector2.down;
        position += velocity * Time.deltaTime;

        timeInCurrentState += Time.deltaTime;
    }

    public void ResetAnimation()
    {
        velocity = Vector3.zero;
        timeAlive = 0.0f;
        timeInCurrentState = 0.0f;
        sprite.enabled = false;
        state = State.SpawnDelay;
    }

    public void SetWorldPositionScale(Vector2 position, Vector2 scale)
    {
        this.position = position;
        this.scale = scale;
        PaintedSpriteConstraint.ConstrainSpriteToWorldPositionScale(position, scale, textureTilesPerMeter, out sprite.offset, out sprite.scale);
    }

    public bool Enabled
    {
        get { return enabled; }
        set 
        { 
            enabled = value;
            //sprite.enabled = enabled;
        }
    }
}
