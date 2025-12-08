using UnityEngine;

public enum TimeOfDayType { Day, Evening, Night }

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    public GameObject levelPrefab;

    public TimeOfDayType timeOfDay;

    public bool windEnabled;
    public Vector2 windDirection = Vector2.right;
    public float windStrength = 3f;
    public bool showWindTutorial;

    public bool enableZoom;
    //public Transform holeTransform;
    public float zoomDistance;
    public float cameraMinY;
    public float cameraMaxY;
}
