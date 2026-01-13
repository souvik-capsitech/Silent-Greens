using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DynamicCameraFit : MonoBehaviour
{
    public float referenceScreenWidth = 1080f;
    public float referenceOrthoSize = 7f;
    public float maxOrthoSize = 12f;

    Camera cam;
    bool levelOverrideApplied = false;

    void Awake()
    {
        Debug.Log("[DynamicCameraFit] Awake");
    }

    void OnEnable()
    {
        Debug.Log("[DynamicCameraFit] OnEnable → subscribe sceneLoaded");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        Debug.Log("[DynamicCameraFit] OnDisable → unsubscribe sceneLoaded");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        Debug.Log("[DynamicCameraFit] Start → schedule ApplyNextFrame + WaitForOverride");
        StartCoroutine(ApplyNextFrame("Start"));
        StartCoroutine(WaitForLevelOverride("Start"));
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[DynamicCameraFit] SceneLoaded → {scene.name} ({mode})");
        levelOverrideApplied = false;

        StartCoroutine(ApplyNextFrame("SceneLoaded"));
        StartCoroutine(WaitForLevelOverride("SceneLoaded"));
    }



    IEnumerator ApplyNextFrame(string caller)
    {
        Debug.Log($"[DynamicCameraFit] {caller} → waiting 1 frame");
        yield return null;

        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("[DynamicCameraFit] Camera.main NOT FOUND");
            yield break;
        }

        Debug.Log($"[DynamicCameraFit] {caller} → Camera found");

        cam.orthographicSize = referenceOrthoSize;
        Debug.Log($"[DynamicCameraFit] {caller} → Reset ortho to {referenceOrthoSize}");

        LevelCameraOverride overrideData = FindFirstObjectByType<LevelCameraOverride>();
        if (overrideData != null)
        {
            Debug.Log($"[DynamicCameraFit] {caller} → LevelCameraOverride FOUND");
            Debug.Log($"[DynamicCameraFit] {caller} → Forced ortho = {overrideData.forcedOrthoSize}");

            cam.orthographicSize = overrideData.forcedOrthoSize;
            levelOverrideApplied = true;
            yield break;
        }

        Debug.Log($"[DynamicCameraFit] {caller} → No level override, using dynamic calc");

        int w = Screen.width;
        int h = Screen.height;

        float aspect = (float)w / h;
        if (aspect < 1f) aspect = 1f / aspect;

        Debug.Log($"[DynamicCameraFit] Screen {w}x{h} | Aspect {aspect:F3}");

        if ((w == 2732 && h == 2048) || (w == 2048 && h == 2732))
        {
            cam.orthographicSize = 7.57f;
            Debug.Log("[DynamicCameraFit] iPad Pro 12.9 detected → ortho 7.57");
            yield break;
        }

        if (Mathf.Abs(aspect - (16f / 9f)) < 0.02f)
        {
            cam.orthographicSize = 5.9f;
            Debug.Log("[DynamicCameraFit] 16:9 detected → ortho 5.9");
            yield break;
        }

        float shortSide = Mathf.Min(w, h);
        float ratio = shortSide / referenceScreenWidth;
        float size = referenceOrthoSize * ratio;
        size = Mathf.Clamp(size, referenceOrthoSize, maxOrthoSize);

        cam.orthographicSize = size;
        Debug.Log($"[DynamicCameraFit] Dynamic final ortho → {size:F3}");
    }

 
    IEnumerator WaitForLevelOverride(string caller)
    {
        cam = Camera.main;
        if (cam == null) yield break;

        while (!levelOverrideApplied)
        {
            LevelCameraOverride overrideData =
                FindFirstObjectByType<LevelCameraOverride>();

            if (overrideData != null)
            {
                cam.orthographicSize = overrideData.forcedOrthoSize;
                levelOverrideApplied = true;

                Debug.Log(
                    $"[DynamicCameraFit] {caller} | Late Level override applied → {overrideData.forcedOrthoSize}"
                );
                yield break;
            }

            yield return null; 
        }
    }
}
