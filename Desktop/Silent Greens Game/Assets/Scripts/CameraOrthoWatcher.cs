using UnityEngine;

public class CameraOrthoWatcher : MonoBehaviour
{
    float lastSize;

    void LateUpdate()
    {
        if (Camera.main == null) return;

        float current = Camera.main.orthographicSize;

        if (!Mathf.Approximately(current, lastSize))
        {
            Debug.Log($"[CameraOrthoWatcher] Ortho changed to {current}");
            lastSize = current;
        }
    }
}
