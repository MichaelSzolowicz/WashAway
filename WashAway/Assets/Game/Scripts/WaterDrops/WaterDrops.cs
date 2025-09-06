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

            // deactivate drop in scene and artist
            newWaterDrop.gameObject.SetActive(false);
            newPaintedSprite.scale = Vector2.zero;
        }
    }

    private WaterDrop CreateWaterDrop(string dropName)
    {
        GameObject newGameObject = new GameObject();

        newGameObject.name = dropName;
        newGameObject.transform.position = transform.position;
        newGameObject.transform.localScale = transform.localScale;

        WaterDrop newWaterDrop = newGameObject.AddComponent<WaterDrop>();

        return newWaterDrop;
    }

    private PaintedSprite CreatePaintedSprite(string spriteTag)
    {
        PaintedSprite newPaintedSprite = new PaintedSprite();
        
        newPaintedSprite.tag = spriteTag;
        newPaintedSprite.sourceTexture = texture;
        artist.AddLayer(newPaintedSprite);

        return newPaintedSprite;
    }

    private void BindPaintedSpriteToObject(GameObject targetObject, string paintedSpriteTag)
    {
        PaintedSpriteConstraint constraint = targetObject.AddComponent<PaintedSpriteConstraint>();
        constraint.Init(artist, paintedSpriteTag, pixelsPerMeter, targetObject.transform);
    }

    private void Update()
    {
        if(paused) return;

        elapsedTime += Time.deltaTime;

        if(elapsedTime >= spawnDelay && numEnabledDrops < numDrops)
        {
            drops[numEnabledDrops].ResetLifetime();
            drops[numEnabledDrops].transform.position = transform.position;
            drops[numEnabledDrops].transform.localScale = transform.localScale;
            drops[numEnabledDrops++].gameObject.SetActive(true);
            elapsedTime = 0;
        }

        for (int i = 0; i < drops.Count; i++)
        {
            WaterDrop drop = drops[i];

            if (!drop.gameObject.activeInHierarchy) break;

            if(drop.TimeAlive > dropLifetime)
            {
                drop.ResetLifetime();
                drop.transform.position = transform.position;
                drop.transform.localScale = transform.localScale;
            }
        }
    }

    public void ResetDrops()
    {
        for (int i = 0;i < drops.Count;i++)
        {
            drops[i].gameObject.SetActive(false);
            paintedSprites[i].scale = Vector2.zero;
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
