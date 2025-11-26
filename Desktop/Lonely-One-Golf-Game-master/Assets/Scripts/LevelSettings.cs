using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public enum TimeOfDay
    {
        Day,
        Evening,
        Night
    }

    public bool enableZoom = false;
    public Transform holeTransform;
    public float zoomDistance = 2f;
    public float cameraMinY = -10f;
    public float cameraMaxY = 10f;

    public TimeOfDay levelTimeOfDay = TimeOfDay.Day; 

    private DayNightManager dayNightManager;

    void Start()
    {
        dayNightManager = FindFirstObjectByType<DayNightManager>();

        if (dayNightManager != null)
        {
            
            switch (levelTimeOfDay)
            {
                case TimeOfDay.Day:
                    dayNightManager.SetTimeOfDay("day");
                    break;
                case TimeOfDay.Evening:
                    dayNightManager.SetTimeOfDay("evening");
                    break;
                case TimeOfDay.Night:
                    dayNightManager.SetTimeOfDay("night");
                    break;
            }
        }
    }
}
