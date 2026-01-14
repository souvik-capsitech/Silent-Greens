using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Shot Settings")]
    public float maxDrag = 4f;
    public float power = 8f;
    public float dragThresholdPixels = 25f; 

    [Header("References")]
    public Rigidbody2D rb;
    public LineRenderer lr;
    public Trajectory trajectory;
    public ParticleSystem impactEffect;
    public TrailRenderer trail;
    public RectTransform cancelButtonRect;
    public Button cancelButton;
    public GameObject restartPanel;

    [Header("Audio")]
    public AudioClip hitBallSFX;

    [Header("Gameplay State")]
    public bool touchedGround = false;
    public bool holeInOnePossible = true;

    private Vector3 dragStartWorld;
    private Vector2 pointerDownScreen;
    private bool pointerDown;
    private bool dragging;
    private bool inputBlocked;

    public int shotsUsed = 0;
    private bool firstHitDone = false;
    private bool ballStoppedAfterFirstShot = false;

    public float cancelTriggerPercent = 0.15f;

    WindManager windManager;

    void Start()
    {
        windManager = FindFirstObjectByType<WindManager>();

        trail.Clear();
        trail.emitting = false;

        shotsUsed = 0;
        holeInOnePossible = true;
        firstHitDone = false;
        ballStoppedAfterFirstShot = false;

        restartPanel.SetActive(false);

        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(CancelShot);
            cancelButton.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (firstHitDone)
            return;
        if (Camera.main == null || inputBlocked)
            return;

    
        if (Input.GetMouseButtonDown(0))
        {
      
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
                return;

            pointerDown = true;
            dragging = false;

            pointerDownScreen = Input.mousePosition;
        }

    
        if (pointerDown && Input.GetMouseButton(0))
        {
            float dragDistance =
                Vector2.Distance(pointerDownScreen, Input.mousePosition);

     
            if (!dragging && dragDistance >= dragThresholdPixels)
            {
                StartDrag();
            }

            if (dragging)
            {
                UpdateDrag();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (dragging)
            {
                FinishShot();
            }

            ResetInputState();
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

    void StartDrag()
    {
        dragging = true;

        dragStartWorld =
            Camera.main.ScreenToWorldPoint(pointerDownScreen);
        dragStartWorld.z = 0;

        lr.positionCount = 1;
        lr.SetPosition(0, dragStartWorld);

        trajectory.Hide();
    }

    void UpdateDrag()
    {
        Vector3 currentWorld =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentWorld.z = 0;

        Vector3 finalDraggingPos =
            2 * dragStartWorld - currentWorld;

        lr.positionCount = 2;
        lr.SetPosition(1, finalDraggingPos);

        Vector3 force = dragStartWorld - currentWorld;
        Vector3 clampedForce =
            Vector3.ClampMagnitude(force, maxDrag) * power;

        trajectory.Show(transform.position, clampedForce);

        float screenY = Input.mousePosition.y / Screen.height;
        cancelButton.gameObject.SetActive(
            screenY < cancelTriggerPercent
        );

        if (RectTransformUtility.RectangleContainsScreenPoint(
            cancelButtonRect,
            Input.mousePosition))
        {
            CancelShot();
        }
    }

    void FinishShot()
    {
        dragging = false;
        pointerDown = false;
        inputBlocked = true;

        lr.positionCount = 0;
        trajectory.Hide();

        cancelButton.gameObject.SetActive(false);

        Vector3 releaseWorld =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
        releaseWorld.z = 0;

        Vector3 force =
            dragStartWorld - releaseWorld;

  
        if (force.magnitude < 0.1f)
        {
            inputBlocked = false;
            return;
        }

        Vector3 clampedForce =
            Vector3.ClampMagnitude(force, maxDrag) * power;

        rb.AddForce(clampedForce, ForceMode2D.Impulse);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(hitBallSFX);

        if (!firstHitDone)
        {
            trail.emitting = true;
            firstHitDone = true;
        }

        shotsUsed++;
        if (shotsUsed > 1)
            holeInOnePossible = false;

        inputBlocked = false;
    }

    void ResetInputState()
    {
        pointerDown = false;
        dragging = false;

        lr.positionCount = 0;
        trajectory.Hide();

        cancelButton.gameObject.SetActive(false);
    }



    IEnumerator HandleFirstShotFail()
    {
        yield return new WaitForSeconds(2f);

        if (!holeInOnePossible)
            yield break;

        LiveManager lm = FindAnyObjectByType<LiveManager>();
        if (lm != null)
            lm.LoseLife();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") &&
            rb.linearVelocity.magnitude > 3f)
        {
            impactEffect.transform.position = transform.position;
            impactEffect.Play();
        }

        if (collision.collider.CompareTag("Ground"))
            touchedGround = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
            StartCoroutine(LifeLoss());
    }

    IEnumerator LifeLoss()
    {
        yield return new WaitForSeconds(0.3f);

        LiveManager lm = FindFirstObjectByType<LiveManager>();
        lm.LoseLife();

        if (lm.currentLives > 0)
        {
            LevelManager levelManager =
                FindFirstObjectByType<LevelManager>();
            levelManager.LoadLevel(levelManager.CurrentLevelIndex);
            ResetBall();
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
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
        trail.emitting = false;

        shotsUsed = 0;
        firstHitDone = false;
        holeInOnePossible = true;
        ballStoppedAfterFirstShot = false;
    }

    void FixedUpdate()
    {
        if (windManager != null &&
            rb.linearVelocity.magnitude > 0.05f)
        {
            rb.AddForce(
                windManager.GetWindForce(),
                ForceMode2D.Force);
        }
    }

    public void CancelShot()
    {
        ResetInputState();
    }
}
