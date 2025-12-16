using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scales a texture by transform's x,y local scale and offsets the texture
/// to be centered on transform's x,y local position. Works in texture space,
/// does not account for UV space or the transform of any object the artist's resulting texture might
/// be applied to. For best results, make sure the UV layout of the object receiving 
/// the artist's resulting texture is right side up and the object faces the viewing camera.
/// </summary>
[ExecuteInEditMode]
public class PaintedSpriteConstraintComponent : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private WAArtist artist;
    [SerializeField] private string paintedSpriteTag;

    public PaintedSprite paintedSprite;

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
        if (artist == null)
        {
            paintedSprite = null;
            return;
        }

        paintedSprite = artist.FindSpriteByTag(paintedSpriteTag);

        if (paintedSprite == null)
        {
            Debug.LogWarning(name + " did not find sprite with tag " + paintedSpriteTag + " in " + artist.name);
            return;
        }
    }

    public void UpdateSprite()
    {
        if (parentTransform == null) return;
        if (artist == null) return;
        if (paintedSprite == null) return;

        PaintedSpriteConstraint.ConstrainSpriteToWorldPositionScale(transform.localPosition, transform.localScale, artist.InverseWidthMeters, out paintedSprite.offset, out paintedSprite.scale);
    }
}
