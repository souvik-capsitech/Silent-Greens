//using System;
using System.Collections.Generic;
using UnityEngine;

public class GroundBuilder : MonoBehaviour
{
    [Header("References")]
    public GameObject groundSegmentPrefab;
    public Transform player;

    [Header("Segment Settings")]
    public int segmentCount = 1000;
    public float segmentWidth = 1f;
    public float recycleX = -10f;

    [Header("Height Variation")]
    public float minY = -0.8f;
    public float maxY = 1.2f;
    public float noiseScale = 0.08f;
    public float smoothness = 0.7f;
    public float hillFrequency = 0.15f;

    private List<GroundSegment> segments = new List<GroundSegment>();
    private float noiseOffset = 0f;
    private float targetHeight = 0f;
    private float currentHeight = 0f;
    private bool isCreatingHill = false;
    private int hillSegmentsRemaining = 0;
    private float startX; // Store starting X position

    void Start()
    {
        noiseOffset = Random.Range(0f, 10000f);
        currentHeight = 0f;
        targetHeight = 0f;

        // Calculate starting position
        Camera cam = Camera.main;
        if (cam != null)
        {
            // Get left edge of camera in world space
            float leftEdgeX = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;
            // Start even further left to ensure coverage
            startX = leftEdgeX - 5f;
        }
        else
        {
            // Fallback if no camera
            startX = -10f;
        }

        // Generate segments starting from left edge
        for (int i = 0; i < segmentCount; i++)
        {
            float xPos = startX + (i * segmentWidth);
            float yPos = GetNextHeight(xPos);

            SpawnSegment(xPos, yPos, i > 0 ? segments[i - 1].transform.localPosition.y : yPos);
        }

        if (player != null)
        {
            PositionPlayerOnGround();
        }
    }

    void Update()
    {
        RecycleSegments();
    }

    float GetNextHeight(float worldX)
    {
        // Decide if we should create a hill
        if (!isCreatingHill && Random.value < hillFrequency * 0.01f)
        {
            isCreatingHill = true;
            hillSegmentsRemaining = Random.Range(8, 20);

            float hillDirection = Random.value > 0.5f ? 1f : -1f;
            targetHeight = currentHeight + (Random.Range(0.8f, 2.0f) * hillDirection);
            targetHeight = Mathf.Clamp(targetHeight, minY, maxY);
        }

        // If creating a hill, move toward target
        if (isCreatingHill)
        {
            currentHeight = Mathf.Lerp(currentHeight, targetHeight, 0.08f);
            hillSegmentsRemaining--;

            if (hillSegmentsRemaining <= 0)
            {
                isCreatingHill = false;
            }
        }
        else
        {
            // Normal gentle slope using Perlin noise
            float noise = Mathf.PerlinNoise((worldX + noiseOffset) * noiseScale, 0f);
            targetHeight = Mathf.Lerp(minY, maxY, noise);

            currentHeight = Mathf.Lerp(currentHeight, targetHeight, 1f - smoothness);
        }

        return currentHeight;
    }

    public void PositionPlayerOnGround()
    {
        if (player == null || segments.Count == 0) return;

        GroundSegment closestSegment = null;
        float minDistance = float.MaxValue;

        foreach (GroundSegment segment in segments)
        {
            float distance = Mathf.Abs(segment.transform.position.x - player.position.x);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSegment = segment;
            }
        }

        if (closestSegment != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            float playerHeight = playerCollider != null ? playerCollider.bounds.extents.y : 0.5f;

            Vector3 newPos = player.position;
            newPos.y = closestSegment.transform.position.y + playerHeight + 0.1f;
            player.position = newPos;
        }
    }

    void SpawnSegment(float xPos, float yPos, float previousY)
    {
        GameObject segmentObj = Instantiate(groundSegmentPrefab, transform);
        segmentObj.transform.localPosition = new Vector2(xPos, yPos);

        GroundSegment segment = segmentObj.GetComponent<GroundSegment>();
        if (segment == null)
        {
            segment = segmentObj.AddComponent<GroundSegment>();
        }

        segment.Initialize(segmentWidth, previousY - yPos);
        segments.Add(segment);
    }

    void RecycleSegments()
    {
        // Update recycleX to be relative to camera's left edge
        Camera cam = Camera.main;
        if (cam != null)
        {
            recycleX = cam.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x - 10f;
        }

        for (int i = 0; i < segments.Count; i++)
        {
            GroundSegment segment = segments[i];

            if (segment.transform.position.x < recycleX)
            {
                float rightMostX = GetRightMostX();
                float newX = rightMostX + segmentWidth;
                float newY = GetNextHeight(newX);

                GroundSegment prevSegment = GetSegmentAt(rightMostX);
                float previousY = prevSegment != null ? prevSegment.transform.position.y : newY;

                segment.transform.position = new Vector2(newX, newY);
                segment.Initialize(segmentWidth, previousY - newY);
                segment.Restore();
            }
        }
    }

    float GetRightMostX()
    {
        float maxX = float.MinValue;
        foreach (GroundSegment segment in segments)
        {
            if (segment.transform.position.x > maxX)
                maxX = segment.transform.position.x;
        }
        return maxX;
    }

    GroundSegment GetSegmentAt(float xPos)
    {
        foreach (GroundSegment segment in segments)
        {
            if (Mathf.Abs(segment.transform.position.x - xPos) < 0.1f)
                return segment;
        }
        return null;
    }
}