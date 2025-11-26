using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject[] levels;
    public GameObject ball;
    public int CurrentLevelIndex => currIdx;

    private GameObject currLevel;
    private int currIdx = 0;

    void Start()
    {
        LoadLevel(currIdx);
    }

    public void LoadLevel(int idx)
    {
        if (currLevel != null)
        {
            Destroy(currLevel);
        }

        currLevel = Instantiate(levels[idx]);

      
        DayNightManager dayNight = FindAnyObjectByType<DayNightManager>();
        if (dayNight != null)
        {
            LevelSettings settings = currLevel.GetComponent<LevelSettings>();

            if (settings != null)
            {
        CameraZoom camZoom = FindAnyObjectByType<CameraZoom>();

        if (camZoom != null)
        {
                    camZoom.ResetCamera();

                    
                    if (settings != null && settings.enableZoom)
                    {
                    camZoom.ball = ball.transform;
                    camZoom.hole = settings.holeTransform;         
                    camZoom.zoomDistance = settings.zoomDistance;
                        camZoom.minY = settings.cameraMinY;
                        camZoom.maxY = settings.cameraMaxY;

                    }
                    else
            {
                        camZoom.ball = null;
                        camZoom.hole = null;
                    }
                  
                }
         
                switch (settings.levelTimeOfDay)
                {
                    case LevelSettings.TimeOfDay.Day:
                        dayNight.SetTimeOfDay("day");
                        break;
                    case LevelSettings.TimeOfDay.Evening:
                        dayNight.SetTimeOfDay("evening");
                        break;
                    case LevelSettings.TimeOfDay.Night:
                        dayNight.SetTimeOfDay("night");
                        break;
                }
            }
        }
     

       
        Transform spawn = currLevel.transform.Find("SpawnPoint");
        if (spawn != null)
        {
            ball.transform.position = spawn.position;
        }
        else
        {
            Debug.LogError("SpawnPoint not found in level " + idx);
            return;
        }

        ball.SetActive(true);

        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Dynamic;

        ball.GetComponent<Collider2D>().enabled = true;
        ball.GetComponent<PlayerMovement>().ResetBall();
    }

    public void LoadNextLevel()
    {
        currIdx++;
        if (currIdx >= levels.Length)
            currIdx = 0;

        LoadLevel(currIdx);
    }

    public void RestartLevel()
    {
        LoadLevel(currIdx);
        Time.timeScale = 1f;
    }


 
}
