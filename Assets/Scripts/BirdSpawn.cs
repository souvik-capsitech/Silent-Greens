using UnityEngine;
using System.Collections.Generic;

public class BirdSpawner : MonoBehaviour
{
    public List<GameObject> birdPrefabs;  
    public float spawnX = -12f;
    public float minY = 1f;
    public float maxY = 5f;
    public float spawnInterval = 3f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnBird), 1f, spawnInterval);
    }

    void SpawnBird()
    {
        if (birdPrefabs.Count == 0) return;

        // pick a random prefab
        int index = Random.Range(0, birdPrefabs.Count);
        GameObject prefab = birdPrefabs[index];

        Vector3 pos = new Vector3(spawnX, Random.Range(minY, maxY), 0);
        Instantiate(prefab, pos, Quaternion.identity);
    }
}
