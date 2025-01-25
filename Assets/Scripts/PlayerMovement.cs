using System.Collections;
using UnityEngine;

enum PlayerState
{
    OnGround,
    OffGround,
    Fall
}
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerState state;

    [Header("Walk relate")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float additionalAirSpeed;
    [SerializeField] private float accelRate;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    private float x;

    [Header("Jump relate")]
    [SerializeField] private float playerJumpPower;
    [SerializeField] private float normalGravity;
    [SerializeField] private float peakGravity;
    [SerializeField] [Range(0f,1)] private float hangThreshold;
    private bool isJumping;
    private bool isFalling;

    private float jumpbufferTime = 0.2f;
    private float jumpbufferCounter;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Unchange")]
    [SerializeField] private GameObject playerRightFoot;
    [SerializeField] private GameObject playerLeftFoot;
    [SerializeField] private float jumpToFallDelay;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PlayerAnimation playerAnimation;

    private bool isAirAttack;
    private bool isFallAttack;

    void Start()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        minSpeed = playerSpeed;
        maxSpeed = playerSpeed * 2;
        additionalAirSpeed = playerSpeed * 2;
        normalGravity = rb.gravityScale;
        peakGravity = normalGravity * 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");

        playerSpeed = Mathf.Clamp(playerSpeed, minSpeed, maxSpeed);

        switch (StateDetermined())
        {
            case PlayerState.OnGround:
                PlayerJump();
                playerAnimation.SimpleAttack();

                break;
            case PlayerState.OffGround:
                AirAttack();
                break;
            case PlayerState.Fall:
                FallAirAttack();
                break;
        }

        PeakVelocity();
        JumpExtraFunction();

    }

    private void FixedUpdate()
    {
        if (x != 0)
        {
            PlayerMove();

            playerSpeed += accelRate * Time.deltaTime;

        }
        else
        {
            playerSpeed -= accelRate * Time.deltaTime;
        }

        if (isAirAttack)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y *playerAnimation._airAttackAdditional * Time.deltaTime);
            isAirAttack = false;
        }

        if(isFallAttack)
        {
            rb.velocity = Vector2.down * playerAnimation._fallAttackAdditional * Time.deltaTime;
            isFallAttack = false;
        }
    }
    private void AirAttack()
    {
        if(Input.GetButtonDown("Fire1") && !playerAnimation._isAttacking)
        {
            isAirAttack = true;
            playerAnimation.SimpleAirAttack();
        }
        
    }

    private void FallAirAttack()
    {
        if (Input.GetButtonDown("Fire1") && !playerAnimation._isAttacking)
        {
            isFallAttack = true;
            playerAnimation.FallingAirAttack();
        }
    }
    private PlayerState StateDetermined()
    {
        if (isGrounded())
        {
            state = PlayerState.OnGround;
            isFalling = false;
            rb.gravityScale = normalGravity;
            coyoteTimeCounter = coyoteTime;
        }

        if (!isGrounded() && isJumping)
        {
            state = PlayerState.OffGround;
        }

        if (!isGrounded() && !isJumping && isFalling)
        {
            state = PlayerState.Fall;
            coyoteTimeCounter = -Time.deltaTime;
        }

        return state;
    }

    private void PlayerMove()
    {
        if(rb.gravityScale == normalGravity)
        {
            rb.velocity = new Vector2(x * playerSpeed * Time.deltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(x * additionalAirSpeed * Time.deltaTime, rb.velocity.y);
        }
        
    }

    private void PeakVelocity()
    {
        if(isJumping || isFalling && Mathf.Abs(rb.velocity.y) < hangThreshold)
        {
            rb.gravityScale = peakGravity;
        }
    }

    private void PlayerJump()
    {
        if(coyoteTimeCounter > 0f && jumpbufferCounter > 0f && !isJumping)
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, playerJumpPower);
            coyoteTimeCounter = 0f;
            StartCoroutine(JumpToFalling());
        }
    }

    private IEnumerator JumpToFalling()
    {
        yield return new WaitForSeconds(jumpToFallDelay);
        isJumping = false;
        isFalling = true;
    }

    private void JumpExtraFunction()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpbufferCounter = jumpbufferTime;
        }
        else
        {
            jumpbufferCounter -= Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }
    }
    private bool isGrounded()
    {
        bool leftFoot = Physics2D.Raycast(playerLeftFoot.transform.position, Vector2.down, 0.1f,groundLayer);
        bool rightFoot = Physics2D.Raycast(playerRightFoot.transform.position, Vector2.down, 0.1f, groundLayer);

        if (leftFoot == true || rightFoot == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
