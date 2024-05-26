using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float DistanceCovered;
    public float TimeElapsed;
    public float BestDistance;
    public float BestTime;
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private ObjectPoolManager poolManager;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float laneWidth;

    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpGravity = 20f;
    [SerializeField] private float groundY;
    [SerializeField] private float slideTimer = 0.5f;

    private Vector3 forwardVector = new Vector3(0, 0, 1);
    private BasePlatform previousOverlappingPlatform;
    private BasePlatform currentOverlappingPlatform = null;

    private Vector3 startTouchPosition, currentTouchPosition;
    private Vector3 targetPosition;

    private bool isSwiping;
    private bool isJump;
    private bool isSlide;
    private bool isTurnPlaying = false;

    private float slidingTimer;
    private float slideHeight = 1.2f;
    private float standHeight = 2.2f;
    private float jumpVerticalSpeed = 0f;

    public static float TimeElaped = 0f;
    public static float DistanceCovered = 0f;
    public static float CoinsCollected = 0f;

    private Vector3 capsuleSlidePivot = new Vector3(0f, -0.2f, 0f);
    private Vector3 standCapsulePivot = Vector3.zero;

    private Vector3 leftBound, rightBound;

    private PlayerData playerData;

    // Gameobject Tags
    private const string PLATFORM_OVERLAPPER = "PlatformOverlapper";

    public BasePlatform PreviousOverlappingPlatform
    {
        get => previousOverlappingPlatform;
        set => previousOverlappingPlatform = value;
    }

    private void PerformSlide()
    {
        if (capsuleCollider.height != slideHeight || capsuleCollider.center != capsuleSlidePivot)
        {
            capsuleCollider.height = slideHeight;
            capsuleCollider.center = capsuleSlidePivot;
        }

        animator.SetBool("isSliding", true);
        slidingTimer -= Time.deltaTime;
        if (slidingTimer <= 0)
        {
            isSlide = false;
            slidingTimer = slideTimer;
            animator.SetBool("isSliding", false);
            capsuleCollider.height = standHeight;
            capsuleCollider.center = standCapsulePivot;
        }
    }

    private void TurnHorizontal()
    {
        var dist = transform.position - targetPosition;
        if (Mathf.Abs(dist.x) > 0.1f) // 0.1f
        {
            if (dist.x > 0 && !isTurnPlaying)
            {
                animator.SetTrigger("TurnRight");
                isTurnPlaying = true;
            }
            else if (dist.x < 0 && !isTurnPlaying)
            {
                animator.SetTrigger("TurnLeft");
                isTurnPlaying = true;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 50f);
        }
        else
        {
            isTurnPlaying = false;
        }
    }

    private void PerformJump()
    {
        // Move the player up and down based on verticalSpeed
        var gravityOffset = jumpGravity * Time.deltaTime;
        jumpVerticalSpeed -= gravityOffset;
        transform.position += new Vector3(0, jumpVerticalSpeed * Time.deltaTime, 0);
        animator.SetBool("isJumping", true);

        // Check if the player has landed
        if (transform.position.y <= groundY)
        {
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            isJump = false;
            animator.SetBool("isJumping", false);
            jumpVerticalSpeed = 0;
        }
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Debug.LogError($":: TouchCount");
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isSwiping = true;
                    break;
                case TouchPhase.Moved:
                    currentTouchPosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    if (isSwiping)
                    {
                        isSwiping = false;
                        Vector2 swipeDelta = currentTouchPosition - startTouchPosition;
                        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                        {
                            if (swipeDelta.x > 0 && transform.position.x < rightBound.x - 1)
                                MoveRight();
                            else if (swipeDelta.x < 0 && transform.position.x > leftBound.x + 1)
                                MoveLeft();
                        }
                        else if (Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x))
                        {
                            if (swipeDelta.y > 0)
                            {
                                isJump = true;
                                jumpVerticalSpeed = jumpSpeed;
                            }
                            else if (swipeDelta.y < 0)
                            {
                                isSlide = true;
                            }
                        }
                    }
                    break;
            }
        }

        void MoveRight() => targetPosition = transform.position + (Vector3.right * laneWidth);

        void MoveLeft() => targetPosition = transform.position + (Vector3.left * laneWidth);
    }

    private void SavePlayerData()
    {
        playerData.DistanceCovered = DistanceCovered;
        playerData.TimeElapsed = TimeElaped;

        if (DistanceCovered > playerData.BestDistance)
            playerData.BestDistance = DistanceCovered;
        if (TimeElaped > playerData.BestTime)
            playerData.BestTime = TimeElaped;

        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
    }

    private void ResetData()
    {
        playerData.DistanceCovered = 0;
        playerData.TimeElapsed = 0;
        DistanceCovered = 0;
        TimeElaped = 0;
        CoinsCollected = 0;
    }

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData"));
            ResetData();
        }

        UIManager.Instance.HideAnyActivePopup();
        UIManager.Instance.ShowPopup(UIPanel.HUD);
    }

    void Start()
    {
        standHeight = capsuleCollider.height;
        standCapsulePivot = capsuleCollider.center;
        slidingTimer = slideTimer;

        leftBound = transform.position + (Vector3.left * laneWidth);
        rightBound = transform.position + (Vector3.right * laneWidth);

        if (playerRb == null)
            playerRb = GetComponent<Rigidbody>();

        if (capsuleCollider == null)
            capsuleCollider = GetComponent<CapsuleCollider>();

        targetPosition = transform.position;
        groundY = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (isJump)
        {
            PerformJump();
        }
        else if (isSlide)
        {
            PerformSlide();
        }
    }

    void Update()
    {
        TimeElaped += Time.deltaTime;
        DistanceCovered += moveSpeed * Time.deltaTime;

        transform.Translate(forwardVector * moveSpeed * Time.deltaTime);

        DetectSwipe();
        TurnHorizontal();
    }

    private void OnDisable()
    {
        InitPlayerData();
        SavePlayerData();
    }

    private void OnDestroy()
    {
        InitPlayerData();
        SavePlayerData();
    }

    private void InitPlayerData()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
            playerData = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("PlayerData"));
        else
            playerData = new PlayerData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PLATFORM_OVERLAPPER)
        {
            var overlappingPlatform = other.transform.parent.gameObject;
            previousOverlappingPlatform = currentOverlappingPlatform;
            currentOverlappingPlatform = overlappingPlatform.GetComponent<BasePlatform>();
        }

        if (other.tag == "Coin")
        {
            other.gameObject.SetActive(false);
            //var collectable = other.gameObject.GetComponent<Collectable>();
            //poolManager.PoolCoin(collectable);

            CoinsCollected++;
        }
    }
}
