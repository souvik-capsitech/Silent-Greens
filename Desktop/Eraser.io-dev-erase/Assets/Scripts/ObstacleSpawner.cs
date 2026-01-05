//using System;
//using System.Diagnostics;
//using System;
//using System.Diagnostics;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float spawnInterval = 1.5f;
    public float spawnOffsetBeyondScreen = 1f; // Small distance beyond right edge
    public float difficultyIncreaseRate = 0.001f;

    private Camera mainCamera;
    private float timer;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
            spawnInterval = Mathf.Max(0.6f, spawnInterval - difficultyIncreaseRate);
        }
    }
    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0 || mainCamera == null) return;

        float rightEdgeX = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x;
        float spawnX = rightEdgeX + spawnOffsetBeyondScreen;

        Vector2 rayStart = new Vector2(spawnX, 10f);
        RaycastHit2D hit = Physics2D.Raycast(
            rayStart,
            Vector2.down,
            20f,
            LayerMask.GetMask("Ground")
        );

        if (!hit)
        {
            Debug.LogWarning($"No ground found at X: {spawnX}");
            return;
        }

        float groundY = hit.point.y;

        int index = Random.Range(0, obstaclePrefabs.Length);
        GameObject obstaclePrefab = obstaclePrefabs[index];

        Collider2D prefabCol = obstaclePrefab.GetComponent<Collider2D>();
        if (prefabCol == null)
        {
            Debug.LogError("Obstacle prefab missing Collider2D!");
            return;
        }

        // Spawn at ground level first
        Vector2 spawnPos = new Vector2(spawnX, groundY);
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);

        // Now calculate the actual bottom of the spawned collider
        Collider2D obstacleCol = obstacle.GetComponent<Collider2D>();
        float colliderBottom = obstacleCol.bounds.min.y;

        // Adjust position so collider bottom sits exactly on ground
        float correction = groundY - colliderBottom;
        obstacle.transform.position += Vector3.up * correction;

        Transform worldParent = FindFirstObjectByType<GroundBuilder>()?.transform.parent;
        if (worldParent != null)
        {
            obstacle.transform.SetParent(worldParent, true);
        }
        else
        {
            obstacle.transform.SetParent(FindFirstObjectByType<GroundBuilder>()?.transform, true);
        }
    }

    //void SpawnObstacle()
    //{
    //    if (obstaclePrefabs.Length == 0 || mainCamera == null) return;

    //    // Get the right edge of the camera in world space
    //    float rightEdgeX = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x;

    //    // 
    //    float spawnX = rightEdgeX + spawnOffsetBeyondScreen;

    //    // Raycast DOWN from high above to find the ground at this X position
    //    Vector2 rayStart = new Vector2(spawnX, 10f);
    //    RaycastHit2D hit = Physics2D.Raycast(
    //        rayStart,
    //        Vector2.down,
    //        20f,
    //        LayerMask.GetMask("Ground")
    //    );

    //    if (!hit)
    //    {
    //        Debug.LogWarning($"No ground found at X: {spawnX}");
    //        return;
    //    }

    //    // Get ground surface Y position
    //    float groundY = hit.point.y;

    //    // Select random obstacle
    //    int index = Random.Range(0, obstaclePrefabs.Length);
    //    GameObject obstaclePrefab = obstaclePrefabs[index];

    //    // Get collider to calculate proper positioning
    //    Collider2D prefabCol = obstaclePrefab.GetComponent<Collider2D>();
    //    if (prefabCol == null)
    //    {
    //        Debug.LogError("Obstacle prefab missing Collider2D!");
    //        return;
    //    }


    //    float pivotToBottomOffset = prefabCol.bounds.extents.y + prefabCol.offset.y;

    //    Vector2 spawnPos = new Vector2(spawnX, groundY + pivotToBottomOffset);


    //    GameObject obstacle = Instantiate(obstaclePrefab, spawnPos, Quaternion.identity);
    //    //GameObject obstacle = Instantiate(obstaclePrefab);
    //    //Collider2D col = obstacle.GetComponent<Collider2D>();

    //    //float obstacleBottom = col.bounds.min.y;
    //    //float correction = groundY - obstacleBottom;

    //    //obstacle.transform.position += Vector3.up * correction;

    //    Transform worldParent = FindFirstObjectByType<GroundBuilder>()?.transform.parent;
    //    if (worldParent != null)
    //    {
    //        obstacle.transform.SetParent(worldParent, true);
    //    }
    //    else
    //    {
    //        // Fallback: parent to ground builder itself
    //        obstacle.transform.SetParent(FindFirstObjectByType<GroundBuilder>()?.transform, true);
    //    }

    //    //Debug.Log($"Spawned spike at X:{spawnPos.x}, Y:{spawnPos.y} (Ground Y: {groundY})");
    //}
}
