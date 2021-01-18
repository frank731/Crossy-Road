using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    /*
     * 1 = north
     * 2 = east
     * 3 = south
     * 4 = west
     */
    public int direction = 1;
    public int jumpDist;
    public long score = 0;
    public float cameraFollowSpeed = 0.3f;
    public bool isDead = false;
    public bool onLog = false;
    public bool goingToLog = false;
    public bool isJumping = false;
    public Animator animator;
    public Vector3 beforeJumpLocation;
    public Rigidbody currentLog;
    public CameraFollowPlayer cameraFollowPlayer;
    public AudioClip jumpSFX;
    public AudioClip touchedGroundSFX;
    public AudioClip crushedSFX;
    public AudioClip splashSFX;
    public GameObject splashEffect;
    public UnityEvent moveForward;
    public WaitForSeconds logReanableTime = new WaitForSeconds(0.15f);

    [SerializeField]
    public Rigidbody rb;
    private AudioSource sfxAudioSource;
    private TMPro.TextMeshProUGUI scoreText;
    private readonly float rotateTime = 0.3f;
    private float farthestDist = -1;
    private bool isTurning = false;
    private bool isMoving = false;
    private bool drowned = false;
    private bool goingOffLog = false;
    private GameManager gameManager;

    private void Awake()
    {
        //rb.interpolation = RigidbodyInterpolation.Interpolate;
        gameManager = GameManager.Instance;
        sfxAudioSource = GetComponent<AudioSource>();
        cameraFollowPlayer = gameManager.mainCamera.GetComponent<CameraFollowPlayer>();
        cameraFollowPlayer.player = transform;
        cameraFollowPlayer.playerController = this;
        scoreText = GameObject.FindGameObjectWithTag("ScoreUI").GetComponent<TMPro.TextMeshProUGUI>();
    }
    private bool V3Equal(Vector3 a, Vector3 b)
    {
        //check if two vector3s are equal, needed for turn coroutine as while will infinitely run as normal == requires perfect precision which leads to infinite loop
        return Vector3.SqrMagnitude(a - b) < 0.000001;
    }

    private IEnumerator Turn(bool left)
    {
        isTurning = true;
        float currentRotTime = 0f;
        Vector3 targetAngle;
        Vector3 currentAngle = transform.eulerAngles;
        if (left)
        {
            direction -= 1;
            if(direction == 0)
            {
                direction = 4;
            }
            if (transform.eulerAngles.y == -180)
            {
                targetAngle = new Vector3(0, 90, 0);
            }
            else
            {
                targetAngle = transform.eulerAngles - new Vector3(0, 90, 0);
            }
        }
        else
        {
            direction = direction % 4 + 1;
            targetAngle = transform.eulerAngles + new Vector3(0, 90, 0);
        }
        while (!V3Equal(currentAngle, targetAngle))
        {
            currentRotTime += Time.deltaTime;
            currentAngle = new Vector3(
             Mathf.LerpAngle(currentAngle.x, targetAngle.x, currentRotTime / rotateTime),
             Mathf.LerpAngle(currentAngle.y, targetAngle.y, currentRotTime / rotateTime),
             Mathf.LerpAngle(currentAngle.z, targetAngle.z, currentRotTime / rotateTime));
            transform.eulerAngles = currentAngle;
            yield return null;
        }
        isTurning = false;
    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("Killed");
        sfxAudioSource.PlayOneShot(crushedSFX);
        gameManager.OnDie();
    }

    public void Drown()
    {
        isDead = true;
        drowned = true;
        Instantiate(splashEffect, transform.position, transform.rotation);
        transform.GetChild(0).GetComponent<Collider>().enabled = false;
        sfxAudioSource.PlayOneShot(splashSFX);
        gameManager.OnDie();
    }

    public void Explode()
    {
        isDead = true;
        gameManager.OnDie();
        cameraFollowPlayer.enabled = false;
        Destroy(gameObject);
    }

    public void Move()
    {
        if (!goingToLog && !isDead)
        {
            onLog = false;
            isJumping = false;
            //transform.position = transform.GetChild(0).position;
            transform.Translate(new Vector3(0, 0, jumpDist));
            if (goingOffLog) {
                //snap position to grid
                StartCoroutine(GlobalFunctions.MoveObject(transform, transform.position, new Vector3(SnapValue(transform.position.x, jumpDist), transform.position.y, transform.position.z), 0.1f));
                goingOffLog = false;
            }

            //check if player is above water
            if (Physics.Raycast(transform.position, Vector3.down , out RaycastHit objectHit, 8))
            {
                if (objectHit.collider.CompareTag("Water"))
                {
                    Drown();
                }
            }
            
            cameraFollowPlayer.FollowPlayer(cameraFollowSpeed);

        }
        else
        {
            goingToLog = false;
        }
        sfxAudioSource.PlayOneShot(touchedGroundSFX);
        isMoving = false;
    }


    private void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }

    private float SnapValue(float input, float factor = 1f)
    {
        if (factor <= 0f)
            throw new UnityException("factor argument must be above 0");

        return Mathf.Round(input / factor) * factor;
    }

    private void Jump()
    {
        beforeJumpLocation = transform.position;
        isMoving = true;
        goingToLog = false;
        isJumping = true;
        if(direction == 1)
        {
            moveForward.Invoke();
            MovedForward();
        }
        if (onLog)
        {
            onLog = false;
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            StartCoroutine(LogDR(currentLog.transform));
            switch (direction)
            {
                case (1): 
                    goingOffLog = true;
                    break;
                case (2):
                    goingToLog = true;
                    onLog = true;
                    break;
                case (3):
                    goingOffLog = true;
                    break;
                case (4):
                    goingToLog = true;
                    onLog = true;
                    break;
            }
        }
        animator.SetTrigger("Jump");
        sfxAudioSource.PlayOneShot(jumpSFX, 0.3f);
    }

    private void MovedForward()
    {
        gameManager.CreateRowFront();
        if(transform.position.z > farthestDist)
        {
            farthestDist = transform.position.z + 7; //prevent player from going back and forth to farm points
            score += 1;
        }
        UpdateScoreUI();
    }

    void Update()
    {
        if (!isDead)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
            {
                // Shoot raycast forward to prevent player from jumping into trees
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit objectHit, 8))
                {
                    if (!objectHit.collider.CompareTag("Tree"))
                    {
                        Jump();
                    }
                }
                else
                {
                    Jump();
                }
                
            }
            else if (!isTurning && !isJumping)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    StartCoroutine(Turn(true));
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    StartCoroutine(Turn(false));
                }
            }
        }
        else
        {
            if (drowned)
            {
                transform.Rotate(0, Random.Range(-30f, 30f), 0);
            }
        }
    }

    private void FixedUpdate()
    {
        if (onLog && !isDead)
        {
            if (isJumping)
            {
                rb.velocity = currentLog.velocity; // * 1.25f;
            }
            else
            {
                rb.velocity = currentLog.velocity;
            }

        }
    }

    private IEnumerator LogDR(Transform log) //stops player from getting stuck on log as it will register another collision with the log without this when jumping
    {
        Collider[] colliders = log.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
        yield return logReanableTime;
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }
}
