using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PaintedSpriteConstraint : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private WAArtist artist;
    [SerializeField] private string paintedSpriteTag;

    private PaintedSprite paintedSprite;
    private Vector2 referenceOffset;
    private Vector2 referenceScale;

    private void OnValidate()
    {
        BindSprite();
    }

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
            paintedSprite = null;
            return;
        }

        paintedSprite = artist.FindSpriteByTag(paintedSpriteTag);

        if(paintedSprite == null)
        {
            Debug.LogWarning(name + " did not find sprite with tag " + paintedSpriteTag + " in " + artist.name);
            return;
        }

        referenceOffset = paintedSprite.offset;
        referenceScale = paintedSprite.scale;
    }

    private void UpdateSprite()
    {
        if (parentTransform == null) return;
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
