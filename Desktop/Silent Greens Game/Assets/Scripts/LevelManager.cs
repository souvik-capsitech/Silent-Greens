using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public GameObject ball;
    public GameObject gameCompletedPanel;
    public GameObject tutorialRoot;

    [Header("Level Data")]
    public LevelData[] levels;

    private int currentIndex;
    private GameObject currentPrefabInstance;

    public int CurrentLevelIndex => currentIndex;

    private void Start()
    {

        currentIndex = LevelLoader.levelToLoad;

        if (currentIndex < 0 || currentIndex >= levels.Length)
            currentIndex = 0;

        LoadLevel(currentIndex);
    }

 
    public void LoadLevel(int index)
    {

        if (currentPrefabInstance != null)
            Destroy(currentPrefabInstance);

        LevelData data = levels[index];


        currentPrefabInstance = Instantiate(data.levelPrefab);

        DayNightManager dayNight = FindAnyObjectByType<DayNightManager>();
        if (dayNight != null)
        {
            dayNight.SetTimeOfDay(data.timeOfDay);
        }

        WindManager wind = FindAnyObjectByType<WindManager>();
        if (wind != null)
        {
            wind.windEnabled = data.windEnabled;
            wind.windDirection = data.windDirection.normalized;
            wind.windStrength = data.windStrength;
        }

    
        Transform spawn = currentPrefabInstance.transform.Find("SpawnPoint");

        if (spawn == null)
        {
            Debug.LogError("SpawnPoint missing in: " + data.name);
            return;
        }

        ball.transform.position = spawn.position;
        ball.SetActive(true);


        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Dynamic;

        ball.GetComponent<Collider2D>().enabled = true;
        ball.GetComponent<PlayerMovement>().ResetBall();

     
   

        ////CameraZoom camZoom = FindAnyObjectByType<CameraZoom>();

        //if (camZoom != null)
        //{
        //    //camZoom.ResetCamera();

        //    if (data.enableZoom)
        //    {
        //        Transform hole = currentPrefabInstance.transform.Find("Hole");

        //        if (hole == null)
        //            Debug.LogError("Hole not found in prefab: " + data.levelPrefab.name);

        //        camZoom.ball = ball.transform;
        //        camZoom.hole = hole;
        //        camZoom.zoomDistance = data.zoomDistance;
        //        camZoom.minY = data.cameraMinY;
        //        camZoom.maxY = data.cameraMaxY;
        //    }
        //    else
        //    {
        //        camZoom.ball = null;
        //        camZoom.hole = null;
        //    }
        //}


        tutorialRoot.SetActive(index == 0 && !TutorialManager.IsTutorialShown);

        if (data.showWindTutorial)
        {
            Transform triangle = currentPrefabInstance.transform.Find("Triangle");
            bool windTutSeen = PlayerPrefs.GetInt("WindTutorialShown", 0) == 1;

            if (triangle != null && !windTutSeen)
            {
                StartCoroutine(PlayWindTutorial(triangle.gameObject));
            }
        }
    }

    private IEnumerator PlayWindTutorial(GameObject triangle)
    {
        yield return new WaitForSeconds(1f);

        WindTutorialManager tut = FindAnyObjectByType<WindTutorialManager>();
        if (tut != null)
        {
            tut.PlayWindTutorial(
                triangle,
                "Watch out! The arrow shows the direction of wind.\nThe wind can push your ball off course."
            );
        }
    }


    public void OnLevelCompleted()
    {
        LevelProgress.UnlockNextLevel(currentIndex);

        if (currentIndex == levels.Length - 1)
        {
            // Last level
            if (gameCompletedPanel != null)
                gameCompletedPanel.SetActive(true);
            return;
        }

        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        currentIndex++;

        if (currentIndex >= levels.Length)
            currentIndex = 0;

        LoadLevel(currentIndex);
    }

    public void RestartLevel()
    {
        LoadLevel(currentIndex);
        Time.timeScale = 1f;
    }
}
