using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float maxDrag = 4f;
    public float power = 8f;
    public float dragThresholdPixels = 25f;


    [Header("Drag Smoothing")]
    public float dragSmoothSpeed = 20f;

    private Vector3 smoothDragPos;


    public Rigidbody2D rb;
    public LineRenderer lr;
    public Trajectory trajectory;
    Vector3 dragStartPos;
    bool dragging = false;
    bool pointerDown = false;

    public ParticleSystem impactEffect;
    public RectTransform cancelButtonRect;
    public FingerTrajectoryTutorial tutorial;
    public int shotsUsed = 0;

    private bool firstHitDone = false;
    private bool hasShot = false;
    private bool inputBlocked = false;

    public TrailRenderer trail;
    public GameObject restartPanel;
    public bool touchedGround = false;
    public bool holeInOnePossible = true;
    public bool ballStoppedAfterFirstShot = false;

    public Button cancelButton;
    public float cancelTriggerPercent = 0.15f;
    public AudioClip hitBallSFX;

    WindManager windManager;
    Vector2 pointerDownScreen;

    void Start()
    {
        windManager = FindFirstObjectByType<WindManager>();
        trail.Clear();
        trail.emitting = false;

        shotsUsed = 0;
        holeInOnePossible = true;
        restartPanel.SetActive(false);

        if (cancelButton != null)
        {
            cancelButton.onClick.AddListener(CancelShot);
            cancelButton.gameObject.SetActive(false);
        }

        smoothDragPos = dragStartPos;

    }

    void Update()
    {
        if (Camera.main == null || inputBlocked)
            return;

        if (!hasShot && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
                return;

            pointerDown = true;
            pointerDownScreen = Input.mousePosition;
        }

        if (pointerDown && Input.GetMouseButton(0))
        {
            float dragDist =
                Vector2.Distance(pointerDownScreen, Input.mousePosition);

            if (!dragging && dragDist >= dragThresholdPixels)
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
                FinishShot();

            ResetInput();
        }

        if (hasShot &&
            !ballStoppedAfterFirstShot &&
            rb.linearVelocity.magnitude < 0.1f)
        {
            ballStoppedAfterFirstShot = true;
            StartCoroutine(HandleFirstShotFail());
        }
    }

    void StartDrag()
    {
        dragging = true;

        dragStartPos =
            Camera.main.ScreenToWorldPoint(pointerDownScreen);
        dragStartPos.z = 0;

        lr.positionCount = 1;
        lr.SetPosition(0, dragStartPos);

        trajectory.Hide();
    }



    void UpdateDrag()
    {
        Vector3 targetDragPos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetDragPos.z = 0;

    
        if (Vector3.Distance(smoothDragPos, targetDragPos) < 0.01f)
            return;

        smoothDragPos = Vector3.Lerp(
            smoothDragPos,
            targetDragPos,
            Time.deltaTime * dragSmoothSpeed
        );

        Vector3 dragDir = smoothDragPos - dragStartPos;
        Vector3 clampedForce =
            Vector3.ClampMagnitude(dragDir, maxDrag) * power;

        Vector3 finalPos = dragStartPos + clampedForce;
        lr.positionCount = 2;
        lr.SetPosition(0, dragStartPos);
        lr.SetPosition(1, finalPos);

        trajectory.Show(transform.position, clampedForce);

        float screenY = Input.mousePosition.y / Screen.height;
        cancelButton.gameObject.SetActive(
            screenY < cancelTriggerPercent
        );

        if (RectTransformUtility.RectangleContainsScreenPoint(
            cancelButtonRect, Input.mousePosition))
        {
            CancelShot();
        }
    }

    void FinishShot()
    {
        dragging = false;
        pointerDown = false;

        lr.positionCount = 0;
        trajectory.Hide();
        cancelButton.gameObject.SetActive(false);

        Vector3 releasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        releasePos.z = 0;

        Vector3 force = releasePos - dragStartPos;
        if (force.magnitude < 0.2f)
            return; 

        Vector3 clampedForce = Vector3.ClampMagnitude(force, maxDrag) * power;
        rb.AddForce(clampedForce, ForceMode2D.Impulse);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySFX(hitBallSFX);

        trail.emitting = true;
        hasShot = true;
        firstHitDone = true;
        shotsUsed++;

        if (shotsUsed > 1)
            holeInOnePossible = false;
    }

    void ResetInput()
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

        LiveManager lm = FindFirstObjectByType<LiveManager>();
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
        hasShot = false;
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
        ResetInput();
    }
}
