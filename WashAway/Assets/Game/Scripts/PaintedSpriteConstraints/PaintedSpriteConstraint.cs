using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedSpriteConstraint : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private WAArtist artist;
    [SerializeField] private string paintedSpriteTag;

    private PaintedSprite paintedSprite;
    private Vector2 referenceOffset;
    private Vector2 referenceScale;

    private void Start()
    {
        BindSprite();
    }

    private void Update()
    {
        UpdateSprite();
    }

    private void BindSprite()
    {
        if(artist == null)
        {
            return;
        }

        paintedSprite = artist.FindSpriteByTag(paintedSpriteTag);

        if(paintedSprite == null) return;

        referenceOffset = paintedSprite.offset;
        referenceScale = paintedSprite.scale;
    }

    private void UpdateSprite()
    {
        if (artist == null) return;
        if(paintedSprite == null) return;

        Vector2 scale = referenceScale * new Vector2(1 / parentTransform.localScale.x, 1 / parentTransform.localScale.y);
        Vector2 offset = new Vector2(parentTransform.position.x, parentTransform.position.y);
        offset *= scale;
        offset -= (scale / 2) - new Vector2(0.5f, 0.5f);

        paintedSprite.scale = scale;
        paintedSprite.offset = offset;
    }
}
