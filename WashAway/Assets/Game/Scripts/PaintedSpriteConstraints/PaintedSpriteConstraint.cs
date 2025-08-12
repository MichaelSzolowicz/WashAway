using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedSpriteConstraint : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private WAArtist artist;
    [SerializeField] private string paintedSpriteTag;

    private PaintedSprite paintedSprite;

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
    }

    private void UpdateSprite()
    {
        if (artist == null) return;
        if(paintedSprite == null) return;

        Vector2 scale = new Vector2(paintedSprite.scale.x * parentTransform.localScale.x, 
            paintedSprite.scale.y * parentTransform.localScale.y);
        Vector2 offset = paintedSprite.offset;
        offset *= scale;
        offset -= (scale / 2) + new Vector2(.5f, .5f);

        paintedSprite.scale = scale;
        paintedSprite.offset = offset;
    }
}
