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
        for (int i = 0; i < numDrops; i++)
        {
            string spriteName = "drop" + i;

            GameObject newDrop = new GameObject();
            newDrop.name = spriteName;
            newDrop.transform.localScale = transform.localScale;
            newDrop.transform.position = transform.position;
            WaterDrop newDropComponent = newDrop.AddComponent<WaterDrop>();
            drops.Add(newDropComponent);
            newDropComponent.Init();

            PaintedSprite newPaintedSprite = new PaintedSprite();
            newPaintedSprite.tag = spriteName;
            newPaintedSprite.sourceTexture = texture;
            artist.AddLayer(newPaintedSprite);
            paintedSprites.Add(newPaintedSprite);

            PaintedSpriteConstraint constraint = newDrop.AddComponent<PaintedSpriteConstraint>();
            constraint.Init(artist, spriteName, pixelsPerMeter, newDrop.transform);

            newDrop.SetActive(false);
            newPaintedSprite.scale = Vector2.zero;
        }

        //drops[0].enabled = true;
        //++numEnabledDrops;
    }

    private void Update()
    {
        if(paused) return;

        elapsedTime += Time.deltaTime;

        if(elapsedTime >= spawnDelay && numEnabledDrops < numDrops)
        {
            drops[numEnabledDrops].Reset();
            drops[numEnabledDrops].transform.position = transform.position;
            drops[numEnabledDrops++].gameObject.SetActive(true);
            elapsedTime = 0;
        }

        for (int i = 0; i < drops.Count; i++)
        {
            WaterDrop drop = drops[i];

            if (!drop.gameObject.activeInHierarchy) break;

            if(drop.TimeAlive > dropLifetime)
            {
                drop.Reset();
                drop.transform.position = transform.position;
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
