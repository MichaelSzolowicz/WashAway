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
public class PaintedSpriteConstraint : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private WAArtist artist;
    [SerializeField] private float pixelsPerMeter;
    [SerializeField] private string paintedSpriteTag;

    private PaintedSprite paintedSprite;

    public void Init(WAArtist artist, string paintedSpriteTag, float pixelsPerMeter, Transform parentTransform)
    {
        this.parentTransform = parentTransform;
        this.artist = artist;
        this.pixelsPerMeter = pixelsPerMeter;
        this.paintedSpriteTag = paintedSpriteTag;

        BindSprite();
    }

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
    }

    public void UpdateSprite()
    {
        if (parentTransform == null) return;
        if (artist == null) return;
        if (paintedSprite == null) return;

        Vector2 scale = parentTransform.localScale;
        scale.x = scale.x == 0 ? 0 : 1 / scale.x;
        scale.y = scale.y == 0 ? 0 : 1 / scale.y;
        
        Vector2 offset = new Vector2(parentTransform.localPosition.x, parentTransform.localPosition.y) * pixelsPerMeter / artist.Size;
        //print(offset);
        offset *= scale;
        offset += (scale / 2) - new Vector2(0.5f, 0.5f);

        paintedSprite.scale = scale;
        paintedSprite.offset = -offset;
    }
}
