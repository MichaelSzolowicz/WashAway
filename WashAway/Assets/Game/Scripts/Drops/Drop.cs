using UnityEngine;

public class Drop
{
    private const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;

    // config
    private PaintedSprite sprite;
    private float textureTilesPerMeter;

    // state
    private bool enabled = false;

    // animation
    private float growTime = 6;
    private float gravityScale = .6f;
    private float growRate = .02f;
    private Vector2 velocity = Vector3.zero;
    private Vector2 position;
    private Vector2 scale;

    private float timeAlive = 0.0f;
    public float TimeAlive { get { return timeAlive; } }

    public Drop(PaintedSprite sprite, float textureTilesPerMeter)
    {
        this.sprite = sprite;
        sprite.enabled = enabled;

        this.textureTilesPerMeter = textureTilesPerMeter;
    }

    public void Update(float deltaTime)
    {
        if(!enabled) return;

        timeAlive += Time.deltaTime;

        if (timeAlive < growTime)
        {
            scale = scale + growRate * Time.deltaTime * Vector2.one;
        }
        else
        {
            velocity += gravityScale * ACCELERATION_DUE_TO_GRAVITY * Time.deltaTime * Vector2.down;
            position += velocity * Time.deltaTime;
        }

        PaintedSpriteConstraint.ConstrainSpriteToWorldPositionScale(position, scale, textureTilesPerMeter, out sprite.offset, out sprite.scale);
    }

    public void ResetAnimation()
    {
        velocity = Vector3.zero;
        timeAlive = 0.0f;
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
            sprite.enabled = enabled;
        }
    }
}
