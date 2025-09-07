using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Drop
{
    private const float ACCELERATION_DUE_TO_GRAVITY = 9.8f;

    private PaintedSprite sprite;
    private float textureTilesPerMeter;

    private float timeAlive = 0.0f;
    private bool enabled = false;

    private float growTime = 1.1f;
    private float gravityScale = .1f;
    private float growRate = .025f;
    private Vector2 velocity = Vector3.zero;

    private Vector2 position;
    private Vector2 scale;

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

    public float TimeAlive { get { return timeAlive; } }
}
