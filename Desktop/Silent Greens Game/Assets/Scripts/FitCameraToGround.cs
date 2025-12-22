using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicCameraFit : MonoBehaviour
{
    public float referenceScreenWidth = 1080f;
    public float referenceOrthoSize = 5f;
    public float maxOrthoSize = 12f;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
        Debug.Log($"[DynamicCameraFit] Awake | Screen: {Screen.width}x{Screen.height}");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log("[DynamicCameraFit] OnEnable");
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log("[DynamicCameraFit] OnDisable");
    }

    void Start()
    {
        Debug.Log("[DynamicCameraFit] Start");
        ApplyCameraSize("Start");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[DynamicCameraFit] SceneLoaded: {scene.name}");
        ApplyCameraSize("SceneLoaded");
    }

    void ApplyCameraSize(string caller)
    {
        if (cam == null)
            cam = Camera.main;

        float shortSide = Mathf.Min(Screen.width, Screen.height);

        float ratio = shortSide / referenceScreenWidth;
        float calculatedSize = referenceOrthoSize * ratio;

        calculatedSize = Mathf.Clamp(calculatedSize, referenceOrthoSize, maxOrthoSize);

        Debug.Log(
            $"[DynamicCameraFit] {caller} | " +
            $"Screen={Screen.width}x{Screen.height}, " +
            $"ShortSide={shortSide}, " +
            $"Ratio={ratio:F3}, " +
            $"Final={calculatedSize:F3}"
        );

        cam.orthographicSize = calculatedSize;
    }

}
