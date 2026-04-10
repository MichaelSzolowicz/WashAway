using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [System.Serializable]
    private class DropSpawnerDebugConfig
    {
        [SerializeField] public bool showDropsCount = false;
    }

    [SerializeField] private Drop dropPrefab;
    [SerializeField] private int numDrops;
    [SerializeField] private float spawnDelay = .5f;
    public bool stopSpawning = false;

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

            GameObject newWaterDrop = Instantiate(dropPrefab.gameObject);
            newWaterDrop.name = dropName;
            drops.Add(newWaterDrop.GetComponent<Drop>());

            newWaterDrop.SetActive(false);
        }
    }


    private void ReleaseFirstDrop()
    {
        if (!drops[first].enabled) return;

        drops[first].gameObject.SetActive(false);

        first = WrapIndex(++first);

        numEnabledDrops--;
    }

    private void SpawnNextDrop()
    {
        if (stopSpawning) return;
        if (drops[next].gameObject.activeInHierarchy) return;
        
        Drop drop = drops[next];
        drop.gameObject.SetActive(true);
        //drop.ResetAnimation();
        drop.transform.position = transform.position;
        drop.transform.localScale = Vector3.zero;

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
        while(nextDrop.gameObject.activeInHierarchy)
        {
            nextDrop.Animate(Time.deltaTime);

            i = WrapIndex(++i);

            if (i == first) break;

            nextDrop = drops[i];
        }

        if (drops[first].TimeAlive > drops[first].Lifetime)
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
