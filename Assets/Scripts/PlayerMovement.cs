using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Fall relate")]
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private float hurtableHeight;
    private float offGroundPoint;
    private float highestPoint;
    private float lowestPoint;

    [Header("Walk relate")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float additionalAirSpeed;
    [SerializeField] private float accelRate;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    private bool isWalking;
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
    [SerializeField] private float coyoteTimeCounter;

    [Header("Unchange")]
    [SerializeField] private GameObject playerRightFoot;
    [SerializeField] private GameObject playerLeftFoot;
    [SerializeField] private float jumpToFallDelay;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject respawnPoint;
    private bool isDead;

    [Header("PlayerAnimation")]
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject healthObject;
    [SerializeField] private Kid _kid;

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
        if(_kid._isWin)
        {
            return;

        }

        healthObject.gameObject.transform.position =  new Vector3(playerObject.transform.position.x,
          playerObject.transform.position.y + 2.28f, playerObject.transform.position.z  );

        if(_playerHealth.GetHealth() <= 0)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;

            if(!isDead)
            {
                isDead = true;
                StartCoroutine(PlayerDead());
            }
            
            return;
        }

        x = Input.GetAxisRaw("Horizontal");
        playerSpeed = Mathf.Clamp(playerSpeed, minSpeed, maxSpeed);

        isLeftOrRight();


        if(!isGrounded() && !isJumping)
        {
            isFalling = true;

            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -50));
        }

        if(!isGrounded())
        {
            offGroundPoint = playerObject.transform.position.y;
            

            if(offGroundPoint > highestPoint)
            {
                
                highestPoint = offGroundPoint;
            }
        }

        if(isGrounded() && isFalling)
        {
            
            isFalling = false;
            lowestPoint = playerObject.transform.position.y;
            
            float result = highestPoint - lowestPoint;
            if(result > hurtableHeight)
            {
                
                _playerHealth.DecreaseHealth(Mathf.RoundToInt(result * 0.7f));
            }

            lowestPoint = 0;
            highestPoint = 0;
        }

        if (isGrounded())
        {
           
            isJumping = false;
            isFalling = false;
        }

        if (isGrounded() && !isJumping)
        {
            coyoteTimeCounter = coyoteTime;
            rb.gravityScale = normalGravity;
        }

        if (coyoteTimeCounter >= -3f)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        PlayerJump();
        PeakVelocity();
        JumpExtraFunction();

        if(x == 0)
        {
            isWalking = false;
        }

        playerAnimation.WalkSequence(isWalking);
        playerAnimation.Jumping(isJumping);
        playerAnimation.isFalling(isFalling);

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

    }

    private IEnumerator PlayerDead()
    {
        yield return new WaitForSeconds(2f);
        playerObject.transform.position = respawnPoint.transform.position;
        yield return new WaitForSeconds(0.5f);
        isDead = false;
        _playerHealth.IncreaseHealth(100);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void PlayerMove()
    {
        if(rb.gravityScale == normalGravity)
        {
            rb.velocity = new Vector2(x * playerSpeed * Time.deltaTime, rb.velocity.y);
            isWalking = true;
        }
        else
        {
            isWalking = false;
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

    private void isLeftOrRight()
    {
        if(x != 0)
        {
            playerObject.transform.localScale = new Vector3(x,1,1);
        }
    }


}
