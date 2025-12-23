using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float maxDrag = 4f;
    public float power = 8f;
    public Rigidbody2D rb;
    public LineRenderer lr;
    public Trajectory trajectory;
    Vector3 dragStartPos;
    bool dragging = false;
    public ParticleSystem impactEffect;
    public RectTransform cancelButtonRect;
    public FingerTrajectoryTutorial tutorial;
    public int shotsUsed = 0;
    private bool firstHitDone = false;
    private bool dragApplied = false;
    public TrailRenderer trail;
    private bool inputBlocked = false;

    public GameObject restartPanel;
    public bool touchedGround = false;
    public bool holeInOnePossible = true;

    public bool ballStoppedAfterFirstShot = false;

    public Button cancelButton;
    public float cancelTriggerPercent = 0.15f;

    WindManager windManager;

    void Start()
    {
        windManager = FindFirstObjectByType<WindManager>();
        trail.emitting = true;
        firstHitDone = false;
        trail.Clear();
        shotsUsed = 0;
        holeInOnePossible = true;
        restartPanel.SetActive(false);

        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(CancelShot);
            cancelButton.gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
   

        if (Camera.main == null)
        {
            enabled = false;
            return;
        }
        if (dragging)
        {
            Vector3 draggingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 finalDraggingPos = 2 * dragStartPos - draggingPos;

            lr.positionCount = 2;
            lr.SetPosition(1, finalDraggingPos);

            Vector3 force = dragStartPos - draggingPos;
            Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;
            trajectory.Show(transform.position, clampedForce);

            float screenY = Input.mousePosition.y / Screen.height;
            if (screenY < cancelTriggerPercent)
            {
                cancelButton.gameObject.SetActive(true);
            }
            else
            {
                cancelButton.gameObject.SetActive(false) ;
            }

                Vector2 fingerPos = Input.mousePosition;
            if (RectTransformUtility.RectangleContainsScreenPoint(cancelButtonRect, fingerPos))
            {
                CancelShot();
                return;
            }
        }

        if (!dragging && !inputBlocked && Input.GetMouseButtonDown(0))

        {
            FingerTrajectoryTutorial tut = FindFirstObjectByType<FingerTrajectoryTutorial>();
            if (tut != null)
            {
                tut.StopTutorial();
                TutorialManager.IsTutorialShown = true;   
            }

          
            dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dragStartPos.z = 0;

            dragging = true;

            lr.positionCount = 1;
            lr.SetPosition(0, dragStartPos);

            trajectory.Hide();
        }


        if(Input.GetMouseButtonUp(0) && dragging)
{
            FinishShot();
        }


        if (!ballStoppedAfterFirstShot &&
     firstHitDone &&
     rb.linearVelocity.magnitude < 0.1f &&
     shotsUsed == 1)
        {
            ballStoppedAfterFirstShot = true;
            StartCoroutine(HandleFirstShotFail());
        }

    }
    private IEnumerator HandleFirstShotFail()
    {
        yield return new WaitForSeconds(2f); 
       
        if (!holeInOnePossible) yield break;

        LiveManager lm = FindAnyObjectByType<LiveManager>();
        if (lm != null)
            lm.LoseLife();
    }


    public void OnGameResumed()
    {
        dragging = false;
        inputBlocked = true;

        lr.positionCount = 0;
        trajectory.Hide();

        if (cancelButton != null)
            cancelButton.gameObject.SetActive(false);

        Input.ResetInputAxes();
        StartCoroutine(UnblockNextFrame());
    }

    IEnumerator UnblockNextFrame()
    {
        yield return null;
        inputBlocked = false;
    }



    void FinishShot()
    {
        ballStoppedAfterFirstShot = false;
        inputBlocked = true;
        trajectory.Hide();
        lr.positionCount = 0;
        dragging = false;

        if (cancelButton != null)
            cancelButton.gameObject.SetActive(false);

        Vector3 dragReleasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 force = dragStartPos - dragReleasePos;
        Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;

        if (rb.linearVelocity.magnitude <= 0.5f)
        {
            rb.AddForce(clampedForce, ForceMode2D.Impulse);

            if (!firstHitDone)
            {
                trail.emitting = true;
                firstHitDone = true;
            }
            shotsUsed++;
            if (shotsUsed > 1)
                holeInOnePossible = false;

        }

        trajectory.Hide();
    }

    public void CancelInputOnPause()
    {
        dragging = false;
        //inputBlocked = true;

        lr.positionCount = 0;
        trajectory.Hide();

        if (cancelButton != null)
            cancelButton.gameObject.SetActive(false);
    }

    public void CancelShot()
    {
        dragging = false;
        lr.positionCount = 0;
        trajectory.Hide();

        if (cancelButton != null)
            cancelButton.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && rb.linearVelocity.magnitude > 3f)
        {
            impactEffect.transform.position = transform.position;
            impactEffect.Play();
        }

        if (collision.collider.CompareTag("Ground"))
        {
            touchedGround = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            //OopsPopUp.instance.oopsText.text = "Into the drink!";
            //OopsPopUp.instance.PlayOops();

            StartCoroutine(LifeLoss());
        }
        }

    IEnumerator LifeLoss()
    {
        yield return new WaitForSeconds(0.3f);

        LiveManager lifeManager = FindFirstObjectByType<LiveManager>();
        lifeManager.LoseLife();

        if (lifeManager.currentLives > 0)
        {
           
            LevelManager levelManager = FindFirstObjectByType<LevelManager>();
            levelManager.LoadLevel(levelManager.CurrentLevelIndex);

            ResetBall();
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        trail.emitting = false;

        Time.timeScale = 0f;
        restartPanel.SetActive(true);
    }

    public void ResetBall()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        trail.Clear();
        inputBlocked = false;
        trajectory.Hide();

        firstHitDone = false;
        trail.emitting = false;
        shotsUsed = 0;
        holeInOnePossible =  true;
        ballStoppedAfterFirstShot = false;
    }
     void FixedUpdate()
    {
        if (windManager != null && rb.linearVelocity.magnitude > 0.05f)
        {
            rb.AddForce(windManager.GetWindForce(), ForceMode2D.Force);

        }
    }


    public void BlockInputForSeconds(float seconds)
{
    StartCoroutine(BlockInputRoutine(seconds));
}

private IEnumerator BlockInputRoutine(float seconds)
{
    inputBlocked = true;
    dragging = false;

    lr.positionCount = 0;
    trajectory.Hide();

    float t = 0f;
    while (t < seconds)
    {
        t += Time.unscaledDeltaTime;
        yield return null;
    }

    inputBlocked = false;
}


}
