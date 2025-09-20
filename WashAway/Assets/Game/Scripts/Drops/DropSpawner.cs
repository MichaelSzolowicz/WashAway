using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [System.Serializable]
    private class DropSpawnerDebugConfig
    {
        [SerializeField] public bool showDropsCount = false;
    }

    [SerializeField] private WAArtist maskGenerator;
    [SerializeField] private Texture dropTexture;
    [SerializeField] private int numDrops;
    [SerializeField] private float dropLifetime;
    [SerializeField] private float spawnDelay = .5f;

    [Header("")]
    [SerializeField] private DropSpawnerDebugConfig debug;

    private List<Drop> drops = new List<Drop>();
    private float elapsedTime = 0;

    private int numEnabledDrops = 0;
    private int first = 0;
    private int next = 0;

    private bool paused = false;
    public bool Paused { get { return paused; } set { paused = value; } }

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

            newWaterDrop.Enabled = false;
        }
    }

    private Drop CreateWaterDrop(string dropName)
    {
        PaintedSprite newSprite = new PaintedSprite();
        newSprite.sourceTexture = dropTexture;
        newSprite.tag = dropName;

        maskGenerator.AddLayer(newSprite);

        return new Drop(newSprite, maskGenerator.InverseWidthMeters);
    }

    private void ReleaseFirstDrop()
    {
        drops[first].Enabled = false;

        first = WrapIndex(++first);

        numEnabledDrops--;
    }

    private void SpawnNextDrop()
    {
        if (drops[next].Enabled) return;
        
        Drop drop = drops[next];
        drop.Enabled = true;
        drop.ResetAnimation();
        drop.SetWorldPositionScale(transform.position, transform.localScale);

        next = WrapIndex(++next);

        numEnabledDrops++;
    }

    private void Update()
    {
        if (paused) return;
        if (drops.Count == 0) return;    

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= spawnDelay)
        {
            SpawnNextDrop();
            elapsedTime = 0;
        }

        int i = first;
        Drop nextDrop = drops[i];
        while(nextDrop.Enabled)
        {
            nextDrop.Update(Time.deltaTime);

            i = WrapIndex(++i);

            if (i == first) break;

            nextDrop = drops[i];
        }

        if (drops[first].TimeAlive > dropLifetime)
        {
            // If / when you need to change this so the drops decide when they are dead,
            // keep the list sorted by alive and dead drops then poll the alive drops
            // each update and swap their position in the array you dont increase time 
            // spent in update. Or add an on death callback to each drop and skip update entirely.
            // Or just search the entire array when you need to spawn a new one. This 
            // would be fine if the array is small enough and / or you spawn them infrequently.
            ReleaseFirstDrop();
        }
    }

    public void ResetDrops()
    {
        for (int i = 0; i < drops.Count; i++)
        {
            drops[i].Enabled = false;
        }

        elapsedTime = 0;
        first = 0;
        next = 0;
        numEnabledDrops = 0;
    }

    private void OnGUI()
    {
        if (debug.showDropsCount)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("num drops: " + numEnabledDrops);
            GUILayout.EndVertical();
        }
    }

    private int WrapIndex(int index)
    {
        if(index >= drops.Count)
        {
            index = 0;
        }

        return index;
    }
}
