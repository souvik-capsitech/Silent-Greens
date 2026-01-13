using UnityEngine;

public class CameraOrthoWatcher : MonoBehaviour
{
    float lastSize = -1f;

    void LateUpdate()
    {
        if (Camera.main == null) return;

        float current = Camera.main.orthographicSize;

        if (!Mathf.Approximately(current, lastSize))
        {
            Debug.Log($"[CameraOrthoWatcher] Frame {Time.frameCount} → Ortho changed to {current}");
            lastSize = current;
        }
    }
}
