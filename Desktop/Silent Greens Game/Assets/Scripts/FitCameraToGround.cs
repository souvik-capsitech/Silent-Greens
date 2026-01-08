using UnityEngine;
using UnityEngine.SceneManagement;

public class DynamicCameraFit : MonoBehaviour
{
    public float referenceScreenWidth = 1080f;
    public float referenceOrthoSize = 7f;
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

    //void ApplyCameraSize(string caller)
    //{
    //    if (cam == null)
    //        cam = Camera.main;

    //    int w = Screen.width;
    //    int h = Screen.height;

    //    float aspect = (float)w / h;
    //    if (aspect < 1f) aspect = 1f / aspect;


    //    if ((w == 2732 && h == 2048) || (w == 2048 && h == 2732))
    //    {
    //        cam.orthographicSize = 7.57f;
    //        Debug.Log($"[DynamicCameraFit] {caller} | iPad Pro 12.9 → 7.32");
    //        return;
    //    }


    //    if (Mathf.Abs(aspect - (16f / 9f)) < 0.02f)
    //    {
    //        cam.orthographicSize = 5.9f;
    //        Debug.Log($"[DynamicCameraFit] {caller} | 16:9 → 5.9");
    //        return;
    //    }

    //    // Fallback (dynamic)
    //    float shortSide = Mathf.Min(w, h);
    //    float ratio = shortSide / referenceScreenWidth;
    //    float calculatedSize = referenceOrthoSize * ratio;
    //    calculatedSize = Mathf.Clamp(calculatedSize, referenceOrthoSize, maxOrthoSize);

    //    cam.orthographicSize = calculatedSize;

    //    Debug.Log(
    //        $"[DynamicCameraFit] {caller} | Screen={w}x{h}, Aspect={aspect:F3}, Final={calculatedSize:F3}"
    //    );
    //}



    void ApplyCameraSize(string caller)
    {
        if (cam == null)
            cam = Camera.main;


     
        LevelCameraOverride overrideData = FindFirstObjectByType<LevelCameraOverride>();
        if (overrideData != null)
        {
            cam.orthographicSize = overrideData.forcedOrthoSize;
            Debug.Log($"[DynamicCameraFit] {caller} | Level prefab override → Ortho = {overrideData.forcedOrthoSize}");
            return;
        }


        int w = Screen.width;
        int h = Screen.height;

        float aspect = (float)w / h;
        if (aspect < 1f) aspect = 1f / aspect;

        if ((w == 2732 && h == 2048) || (w == 2048 && h == 2732))
        {
            cam.orthographicSize = 7.57f;
            Debug.Log($"[DynamicCameraFit] {caller} | iPad Pro 12.9 → 7.57");
            return;
        }

        if (Mathf.Abs(aspect - (16f / 9f)) < 0.02f)
        {
            cam.orthographicSize = 5.9f;
            Debug.Log($"[DynamicCameraFit] {caller} | 16:9 → 5.9");
            return;
        }

        float shortSide = Mathf.Min(w, h);
        float ratio = shortSide / referenceScreenWidth;
        float calculatedSize = referenceOrthoSize * ratio;
        calculatedSize = Mathf.Clamp(calculatedSize, referenceOrthoSize, maxOrthoSize);

        cam.orthographicSize = calculatedSize;

        Debug.Log(
            $"[DynamicCameraFit] {caller} | Screen={w}x{h}, Aspect={aspect:F3}, Final={calculatedSize:F3}"
        );
    }




}
