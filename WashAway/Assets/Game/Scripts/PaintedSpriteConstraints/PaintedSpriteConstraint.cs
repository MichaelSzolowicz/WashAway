using UnityEngine;

public class PaintedSpriteConstraint 
{
    static public void ConstrainSpriteToWorldPositionScale (Vector2 worldPosition, Vector2 worldScale, float textureTilesPerMeter, out Vector2 spritePosition, out Vector2 spriteScale)
    {
        spriteScale.x = worldScale.x == 0 ? 0 : 1 / worldScale.x;
        spriteScale.y = worldScale.y == 0 ? 0 : 1 / worldScale.y;

        spritePosition = new Vector2(worldPosition.x, worldPosition.y) * textureTilesPerMeter;
        spritePosition *= spriteScale;
        spritePosition += (spriteScale / 2) - new Vector2(0.5f, 0.5f);
        spritePosition = -spritePosition;
    }
}
