using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDrops : MonoBehaviour
{
    [SerializeField] private WAArtist artist;
    [SerializeField] private Texture texture;
    [SerializeField] private int numDrops;
    [SerializeField] private float dropLifetime;
    private List<WaterDrop> drops = new List<WaterDrop>();
    private List<PaintedSprite> paintedSprites = new List<PaintedSprite>(); 
    [SerializeField] private float pixelsPerMeter;
    private int numEnabledDrops = 0;
    [SerializeField] private float spawnDelay = .5f;

    private float elapsedTime = 0;

    private bool paused = false;
    public bool Paused { get { return paused; } set { SetPaused(value); } }

    private void Start()
    {
        CreateDrops();
    }

    private void CreateDrops()
    {
        for (int i = 0; i < numDrops; i++)
        {
            string dropName = "drop" + i;

            WaterDrop newWaterDrop = CreateWaterDrop(dropName);
            drops.Add(newWaterDrop);

            PaintedSprite newPaintedSprite = CreatePaintedSprite(dropName);
            paintedSprites.Add(newPaintedSprite);

            BindPaintedSpriteToObject(newWaterDrop.gameObject, dropName);

            ReleaseDrop(i);
        }
    }

    private WaterDrop CreateWaterDrop(string dropName)
    {
        GameObject newGameObject = new GameObject();

        newGameObject.name = dropName;

        WaterDrop newWaterDrop = newGameObject.AddComponent<WaterDrop>();

        return newWaterDrop;
    }

    private PaintedSprite CreatePaintedSprite(string spriteTag)
    {
        PaintedSprite newPaintedSprite = new PaintedSprite();
        
        newPaintedSprite.tag = spriteTag;
        newPaintedSprite.sourceTexture = texture;
        newPaintedSprite.enabled = false;
        artist.AddLayer(newPaintedSprite);

        return newPaintedSprite;
    }

    private void BindPaintedSpriteToObject(GameObject targetObject, string paintedSpriteTag)
    {
        PaintedSpriteConstraint constraint = targetObject.AddComponent<PaintedSpriteConstraint>();
        constraint.Init(artist, paintedSpriteTag, pixelsPerMeter, targetObject.transform);
    }

    private void ReleaseDrop(int index)
    {
        WaterDrop drop = drops[index];
        drop.enabled = false;

        PaintedSprite sprite = paintedSprites[index];
        sprite.enabled = false;
    }

    private void SpawnDrop(int index)
    {
        WaterDrop drop = drops[index];
        drop.enabled = true;
        drop.RestartMovement();
        drop.transform.localScale = transform.localScale;
        drop.transform.position = transform.position;

        drop.GetComponent<PaintedSpriteConstraint>().UpdateSprite();

        PaintedSprite sprite = paintedSprites[index];
        sprite.enabled = true;
    }

    private void Update()
    {
        if(paused) return;

        elapsedTime += Time.deltaTime;

        if(elapsedTime >= spawnDelay && numEnabledDrops < numDrops)
        {
            SpawnDrop(numEnabledDrops);
            numEnabledDrops++;
            elapsedTime = 0;
        }

        for (int i = 0; i < drops.Count; i++)
        {
            WaterDrop drop = drops[i];

            if (!drop.enabled) break;

            if(drop.TimeAlive > dropLifetime)
            {
                SpawnDrop(i);
            }
        }
    }

    public void ResetDrops()
    {
        for (int i = 0;i < drops.Count;i++)
        {
            ReleaseDrop(i);
        }

        elapsedTime = 0;
        numEnabledDrops = 0;
    }

    private void SetPaused(bool newPaused)
    {
        if (newPaused == paused) return;

        paused = newPaused;

        for (int i = 0; i < drops.Count; i++)
        {
            drops[i].Paused = paused;
        }
    }
}
