using System.Collections;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public Transform coin;
    public bool zoomThisLevel = false;
    private CameraZoom camZoom;

     void Start()
    {
        camZoom=  Camera.main.GetComponent<CameraZoom>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {

            StartCoroutine(SuckBall(other.gameObject));
        }
    }



    private IEnumerator SuckBall(GameObject ball)
    {
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        ball.GetComponent<Collider2D>().enabled = false;

        Vector3 startPos = ball.transform.position;
        Vector3 endPos = transform.position;

        float t = 0f;
        float duration = 0.4f;   

       
        TrailRenderer trail = ball.GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.minVertexDistance = 0.01f;
            trail.emitting = true;
        }

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;

            ball.transform.position = Vector3.Lerp(startPos, endPos, lerp);

           
            float scale = Mathf.Lerp(1f, 0.3f, lerp);
            ball.transform.localScale = new Vector3(0.2f, 0.2f, 0);

            yield return null;
        }


        if (trail != null)
        {
            trail.emitting = false;
            trail.minVertexDistance = 0.1f;
        }

        ball.SetActive(false);


        ScoreManager.instance.AddDirectShot();

        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(CoinPop());


        LevelManager manager = FindAnyObjectByType<LevelManager>();
        LevelProgress.UnlockNextLevel(manager.CurrentLevelIndex);
        if (manager != null)
        {
            manager.OnLevelCompleted();  
         
        }

    }


    private IEnumerator CoinPop()
    {
        if (coin == null)
            yield break;

      
        coin.gameObject.SetActive(true);
        coin.localScale = new Vector3(0.033f, 0.033f, 1f);

      
        Animator anim = coin.GetComponent<Animator>();
        if (anim != null)
            anim.Play("CoinFlip");

      
        Vector3 startPos = coin.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 0.6f, 0);

        float duration = 0.5f;     
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            coin.localPosition = Vector3.Lerp(startPos, endPos, t / duration);
            yield return null;
        }

       
        yield return new WaitForSeconds(0.2f);
    }

}
