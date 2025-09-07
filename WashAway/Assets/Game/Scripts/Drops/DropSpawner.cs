using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [SerializeField] private WAArtist artist;
    [SerializeField] private Texture texture;
    [SerializeField] private int numDrops;
    [SerializeField] private float dropLifetime;
    [SerializeField] private float pixelsPerMeter;
    [SerializeField] private float spawnDelay = .5f;

    private List<Drop> drops = new List<Drop>();

    private int numEnabledDrops = 0;
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

            Drop newWaterDrop = CreateWaterDrop(dropName);
            drops.Add(newWaterDrop);

            ReleaseDrop(i);
        }
    }

    private Drop CreateWaterDrop(string dropName)
    {
        PaintedSprite newSprite = new PaintedSprite();
        newSprite.sourceTexture = texture;
        newSprite.tag = dropName;

        artist.AddLayer(newSprite);

        return new Drop(newSprite, pixelsPerMeter / artist.Size);
    }

    private void ReleaseDrop(int index)
    {
        Drop drop = drops[index];
        drop.Enabled = false;
    }

    private void SpawnDrop(int index)
    {
        Drop drop = drops[index];
        drop.Enabled = true;
        drop.ResetAnimation();
        drop.SetWorldPositionScale(transform.position, transform.localScale);
    }

    private void Update()
    {
        if (paused) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= spawnDelay && numEnabledDrops < numDrops)
        {
            SpawnDrop(numEnabledDrops);
            numEnabledDrops++;
            elapsedTime = 0;
        }

        for (int i = 0; i < drops.Count; i++)
        {
            Drop drop = drops[i];

            if (!drop.Enabled) break;

            drop.Update(Time.deltaTime);

            if (drop.TimeAlive > dropLifetime)
            {
                SpawnDrop(i);
            }
        }
    }

    public void ResetDrops()
    {
        for (int i = 0; i < drops.Count; i++)
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
    }
}
