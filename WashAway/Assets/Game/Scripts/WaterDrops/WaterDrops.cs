using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrops : MonoBehaviour
{
    [SerializeField] private WAArtist artist;
    [SerializeField] private Texture texture;
    //private List<PaintedSprite> paintedSprites;
    [SerializeField] private float numDrops;
    private List<Transform> drops = new List<Transform>();
    [SerializeField] private float pixelsPerMeter;

    private void Start()
    {
        for (int i = 0; i < numDrops; i++)
        {
            string spriteName = "drop" + i;

            GameObject newDrop = new GameObject();
            newDrop.name = spriteName;
            newDrop.transform.localScale = transform.localScale;

            PaintedSprite newPaintedSprite = new PaintedSprite();
            newPaintedSprite.tag = spriteName;
            newPaintedSprite.sourceTexture = texture;
            artist.AddLayer(newPaintedSprite);

            PaintedSpriteConstraint constraint = newDrop.AddComponent<PaintedSpriteConstraint>();
            constraint.Init(artist, spriteName, pixelsPerMeter, newDrop.transform);
        }
    }

    private void Update()
    {
        
    }
}
