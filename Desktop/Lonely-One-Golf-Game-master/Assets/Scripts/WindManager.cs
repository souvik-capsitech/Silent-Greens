using UnityEngine;

public class WindManager : MonoBehaviour
{
    [Header("Wind Settings")]
    public Vector2 windDirection = Vector2.right;
    public float windStrength = 3f;
    public bool windEnabled = true;

    private void OnValidate()
    {
        if (windDirection.sqrMagnitude > 0.001f)
            windDirection = windDirection.normalized;
    }

    public Vector2 GetWindForce()
    {
        if (!windEnabled)
            return Vector2.zero;

        return windDirection * windStrength;
    }

    // Called by LevelLoader
    public void ApplyWind(bool enabled, Vector2 direction, float strength)
    {
        windEnabled = enabled;

        if (direction.sqrMagnitude > 0.001f)
            windDirection = direction.normalized;

        windStrength = strength;
    }
}
