using UnityEngine;

public class WaterWave : MonoBehaviour
{
    public float speed = 1f;
    public float amplitude = 0.1f;
    public Material material;

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * amplitude;
        material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
