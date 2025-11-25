using UnityEngine;

public class WindManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Vector2 windDirection = Vector2.right;
    public float windStrength = 3f;
    public bool windEnabled = true;


    void OnValidate()
    {
        if(windDirection != Vector2.zero)
            windDirection = windDirection.normalized;
    }
    public Vector2 GetWindForce()
    {
        if(!windEnabled) return Vector2.zero;
        return windDirection * windStrength;
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
