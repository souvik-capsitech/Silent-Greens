//using System;
//using System.Diagnostics;
using UnityEngine;

public class InkBombSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject inkBombPrefab;

    [Header("Spawn Timing")]
    public float spawnInterval = 10f; // Adjust for difficulty
    public float intervalDecreaseRate = 0.001f; // Gets faster over time
    public float minInterval = 0.8f;

    [Header("Spawn Area (Viewport %)")]
    [Range(0f, 1f)]
    public float minScreenX = 0.3f; // Spawn more to the right
    [Range(0f, 1f)]
    public float maxScreenX = 1.0f; // At or beyond right edge
    public float spawnHeightAboveScreen = 1f;

    private Camera mainCamera;
    private float timer;
    private float currentInterval;

    void Start()
    {
        mainCamera = Camera.main;
        currentInterval = spawnInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentInterval)
        {
            SpawnInkBomb();
            timer = 0f;

            // Gradually increase difficulty
            currentInterval = Mathf.Max(minInterval, currentInterval - intervalDecreaseRate);
        }
    }

    void SpawnInkBomb()
    {
        if (inkBombPrefab == null)
        {
            Debug.LogError("InkBomb Prefab not assigned!");
            return;
        }

        if (mainCamera == null) return;

        // Get top of screen in world space
        float topY = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f)).y;
        float spawnY = topY + spawnHeightAboveScreen;

        // Spawn more to the RIGHT side so drift brings them toward player
        float randomViewportX = Random.Range(minScreenX, maxScreenX);
        Vector3 viewportPos = new Vector3(randomViewportX, 1f, mainCamera.nearClipPlane);
        float spawnX = mainCamera.ViewportToWorldPoint(viewportPos).x;

        Vector2 spawnPos = new Vector2(spawnX, spawnY);

        // Spawn bomb independently (not parented)
        GameObject bomb = Instantiate(inkBombPrefab, spawnPos, Quaternion.identity);

        Debug.Log($"Bomb spawned at {spawnPos}");
    }
}
 