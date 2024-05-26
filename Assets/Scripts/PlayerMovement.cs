using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRb;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float laneWidth = 10f;

    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float gravity = 20f;
    [SerializeField] private float verticalSpeed = 0f;
    [SerializeField] private float groundY;

    private Vector3 forwardVector = new Vector3(0, 0, 1);
    private BasePlatform previousOverlappingPlatform;
    private BasePlatform currentOverlappingPlatform = null;

    private Vector3 startTouchPosition, currentTouchPosition;
    private Vector3 targetPosition;

    private bool isSwiping;
    private bool isJump;
    private bool isSlide;

    [SerializeField] private float slideTimer = 0.5f;

    private float slidingTimer;
    private float slideHeight = 1.2f;
    private float standHeight = 2.2f;
    private Vector3 capsuleSlidePivot = new Vector3(0f, -0.2f, 0f);
    private Vector3 standCapsulePivot = Vector3.zero;

    // Gameobject Tags
    private const string PLATFORM_OVERLAPPER = "PlatformOverlapper";

    public BasePlatform PreviousOverlappingPlatform
    {
        get => previousOverlappingPlatform;
        set => previousOverlappingPlatform = value;
    }

    void Start()
    {
        standHeight = capsuleCollider.height;
        standCapsulePivot = capsuleCollider.center;
        slidingTimer = slideTimer;

        if (playerRb == null)
            playerRb = GetComponent<Rigidbody>();

        if (capsuleCollider == null)
            capsuleCollider = GetComponent<CapsuleCollider>();

        targetPosition = transform.position;
        groundY = transform.position.y;
    }

    private bool isTurnPlaying = false;

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

    void Update()
    {
        transform.Translate(forwardVector * moveSpeed * Time.deltaTime);

        DetectSwipe();
        TurnHorizontal();
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
        var gravityOffset = gravity * Time.deltaTime;
        verticalSpeed -= gravityOffset;
        transform.position += new Vector3(0, verticalSpeed * Time.deltaTime, 0);
        animator.SetBool("isJumping", true);
        //Debug.LogWarning($":: gravityOffset: {gravityOffset}");
        //Debug.LogWarning($":: verticalSpeed: {verticalSpeed}");
        //Debug.LogWarning($":: verticalSpeed * Time.deltaTime: {verticalSpeed * Time.deltaTime}");



        // Check if the player has landed
        if (transform.position.y <= groundY)
        {
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            isJump = false;
            animator.SetBool("isJumping", false);
            verticalSpeed = 0;
        }
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
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
                        Debug.LogWarning($":: TouchEnded");
                        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                        {
                            Debug.LogWarning($":: TouchEnded x>y");
                            if (swipeDelta.x > 0)
                                MoveRight();
                            else
                                MoveLeft();
                        }
                        else if (Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x))
                        {
                            Debug.LogWarning($":: TouchEnded y>x :: {swipeDelta.y}");
                            if (swipeDelta.y > 0)
                            {
                                isJump = true;
                                verticalSpeed = jumpSpeed;
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

        void MoveRight()
        {
            targetPosition = transform.position + (Vector3.right * laneWidth);
        }

        void MoveLeft()
        {
            targetPosition = transform.position + (Vector3.left * laneWidth);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == PLATFORM_OVERLAPPER)
        {
            var overlappingPlatform = other.transform.parent.gameObject;

            previousOverlappingPlatform = currentOverlappingPlatform;

            // refactor the use of GetComponent
            currentOverlappingPlatform = overlappingPlatform.GetComponent<BasePlatform>();
            //currentOverlappingPlatform = LevelGenerator.Instance.GetPlatformOverlapper(overlappingPlatform.gameObject.tag);
        }
    }
}
