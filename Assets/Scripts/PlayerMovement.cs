using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathAnim = new Vector2 (10f, 5f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawn;



    Vector2 moveInput;
    Rigidbody2D playerRigidBody;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    float gravityScaleAtStart;
    bool isJumping = false;

    bool isAlive = true;

    [SerializeField] float timerValue = 10f;
    [SerializeField] float deathDelay = 2f;
 
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = playerRigidBody.gravityScale;
    }

  
    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
        isWet();
        isBoing();
        UpdateTimer();
    }
     
    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;
        if (!(timerValue > 0))
        {
            isJumping = false;
        }
    
    }
    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing")))
        {
            return;
        }

        if(value.isPressed)
        {
            isJumping = true;
            FindObjectOfType<AudioManager>().Play("jump");
            playerRigidBody.gravityScale = gravityScaleAtStart;
            playerRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    
    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, playerRigidBody.velocity.y);
        playerRigidBody.velocity = playerVelocity;

        bool playerIsGo = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerIsGo);

    }


    void FlipSprite()
    {
        bool playerIsGo = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerIsGo)
        {
        transform.localScale = new Vector2 (Mathf.Sign(playerRigidBody.velocity.x), 1f);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(bullet, bulletSpawn.position, transform.rotation);
    }
    void isWet()
    {
        if (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            Debug.Log("SPLASH");
            FindObjectOfType<AudioManager>().Play("splash");
            return;
        }
    }

    void isBoing()
    {
        if (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Bouncing")))
        {
            Debug.Log("BOING");
            FindObjectOfType<AudioManager>().Play("boing");
            return;
        }
    }

    // void isClimb() 
    //     {
    //         if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
    //     {
    //         playerRigidBody.gravityScale = gravityScaleAtStart;
    //         playerAnimator.SetBool("isClimbing", false);
    //         return;
    //     }
    //     }

    // void OnClimb(InputValue value)
    // {
    //     if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
    //     {
    //         Debug.Log("Not touching");
    //         playerRigidBody.gravityScale = gravityScaleAtStart;
    //         playerAnimator.SetBool("isClimbing", false);
    //         return;
    //     }
    
    //     {
    //         Debug.Log("touching");
    //      Vector2 climbVelocity = new Vector2(playerRigidBody.velocity.x, moveInput.y * climbSpeed);
    //     playerRigidBody.velocity = climbVelocity;
    //     playerRigidBody.gravityScale = 0;

    //     bool playerHasVerticalSpeed = Mathf.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon;
    //     bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
    //     bool playerMoreVertical = Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x);
    //     playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed && playerMoreVertical);
    //     }
    // }

    void ClimbLadder(){
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
            playerRigidBody.gravityScale = gravityScaleAtStart;
            playerAnimator.SetBool("isClimbing", false);
            return;
        }
        Vector2 climbVelocity = new Vector2(playerRigidBody.velocity.x, moveInput.y * climbSpeed);
        playerRigidBody.velocity = climbVelocity;
        playerRigidBody.gravityScale = 0;

        bool playerHasVerticalSpeed = Mathf.Abs(playerRigidBody.velocity.y) > Mathf.Epsilon;
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidBody.velocity.x) > Mathf.Epsilon;
        bool playerMoreVertical = Mathf.Abs(moveInput.y) > Mathf.Abs(moveInput.x);
        playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed && playerMoreVertical);
    }

    void Die()
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            playerAnimator.SetTrigger("Dying");
            FindObjectOfType<AudioManager>().Play("death");
            playerRigidBody.velocity = deathAnim;
            StartCoroutine(DelayOnDeath(deathDelay));

        }
    }

    IEnumerator DelayOnDeath(float delay){
        yield return new WaitForSeconds(delay);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
