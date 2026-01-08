using UnityEngine;

public class PortalController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform destination;
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Ball");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))

        {

            if(Vector2.Distance(player.transform.position, transform.position)>0.3f)
            {
            player.transform.position = destination.transform.position;

                TrailRenderer trail = player.GetComponent<TrailRenderer>();
                if (trail != null)
                {
                    trail.Clear();
                }
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
