using System.Collections;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;

    [SerializeField] private float spawnFrequency = 1;

    private Coroutine spawnCoroutine;

    private void Start()
    {
        StartSpawning();
    }

    private IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnFrequency);
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }
    }

    public void StartSpawning()
    {
        spawnCoroutine = StartCoroutine(Spawn());
    }

    public void StopSpawning()
    {
        StopCoroutine(spawnCoroutine);
    }
}
