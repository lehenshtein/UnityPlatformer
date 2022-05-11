using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{   
    // GameSession gameSession;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator animator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    CircleCollider2D myHeadCollider;
    LayerMask groundLayer;
    LayerMask climbingLayer;
    [SerializeField] float speedModifier = 5f;
    [SerializeField] float jumpSpeed = 16f;
    [SerializeField] float climbModifier = 4f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    float playerGravityScale;
    bool isAlive = true;
    float fireRate = 1f;
    float counter = 0f;
    bool shooted = false;

    void Awake() {
        // gameSession = FindObjectOfType<GameSession>();
        myRigidbody = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myHeadCollider = GetComponent<CircleCollider2D>();
        groundLayer = LayerMask.GetMask("Ground");
        climbingLayer = LayerMask.GetMask("Climbing");
    }

    void Start() {
        playerGravityScale = myRigidbody.gravityScale;
    }

    void Update() {
        if (!isAlive) {
            return;
        }
        Run();
        FlipSprite();
        Swimming();
        ClimbLedder();
        UpdateTimer();
        Die();
    }

    void Run()
    {
        Vector2 playerVelocity;
        playerVelocity = new Vector2(moveInput.x * speedModifier, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerHasHorizontalSpeed);
    }
    
    void OnMove(InputValue value) {
        if (!isAlive) {
            return;
        }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value) {
        if (!isAlive) {
            return;
        }
        bool isOnGround = myFeetCollider.IsTouchingLayers(groundLayer);
        if (value.isPressed && isOnGround) {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value) {
        if (!isAlive) {
            return;
        }

        if (!shooted) {
            animator.Play("Shooting");
            Instantiate(bullet, gun.position, bullet.transform.rotation);
            counter = fireRate;
            shooted = true;
        }
    }

    void FlipSprite() {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), transform.localScale.y);
        }
        
        // if (moveInput.x > 0) {
        //     transform.localScale = new Vector2(1, transform.localScale.y);
        // }
        // if (moveInput.x < 0) {
        //     transform.localScale = new Vector2(-1, transform.localScale.y);
        // }
    }

    void ClimbLedder() {
        bool isClimbing = myBodyCollider.IsTouchingLayers(climbingLayer);

        if (isClimbing) {
            Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbModifier);
            myRigidbody.velocity = climbVelocity;
            myRigidbody.gravityScale = 0;
            bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
            animator.SetBool("isClimbing", playerHasVerticalSpeed);
            return;
        }

        myRigidbody.gravityScale = playerGravityScale;
        animator.SetBool("isClimbing", false);
    }

    void Die() {
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"))) {
            myRigidbody.velocity = new Vector2(-myRigidbody.velocity.x * 5, 20f);
            isAlive = false;
            animator.SetTrigger("isDying");
            // gameSession.ProcessPlayerDeath();
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void Swimming() {
        Vector2 playerVelocity;
        bool isSwimming = false;
        bool isHeadOverWater = true;
        
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Water"))) {
            isSwimming = true;

            if (myHeadCollider.IsTouchingLayers(LayerMask.GetMask("Water"))) {
                isHeadOverWater = false;
            } else {
                isHeadOverWater = true;
            }
        } else {
            isHeadOverWater = true;
            isSwimming = false;
        }
        
        if (isSwimming && !isHeadOverWater) {
            playerVelocity = new Vector2((moveInput.x * speedModifier) / 2, moveInput.y * climbModifier);
            myRigidbody.velocity = playerVelocity;
        } else if (isSwimming && isHeadOverWater) {
            playerVelocity = new Vector2((moveInput.x * speedModifier) / 2, myRigidbody.velocity.y);
            myRigidbody.velocity = playerVelocity;
        }
    }

    void UpdateTimer() {
        if (shooted) {
            counter -= Time.deltaTime;
            if (counter < 0) {
                shooted = false;
        }
        }
    }

}
