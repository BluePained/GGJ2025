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

    [Header("Jump relate")]
    [SerializeField] private float playerJumpPower;
    [SerializeField] private float normalGravity;
    [SerializeField] private float peakGravity;
    [SerializeField] private float coyoteTimeCouter;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferCounter;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private float airSpeed;
    private bool isJumping;

    [Header("Unchange")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject playerFoot;
    private float x;

    void Start()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        normalGravity = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");

        if (jumpBufferCounter >= -3f)
        {
            coyoteTimeCouter -= Time.deltaTime;
        }

        if (jumpBufferCounter >= -1f)
        {
            jumpBufferCounter -= Time.deltaTime;
        }
            

        if (isGrounded())
        {
            state = PlayerState.OnGround;
        }

        if(!isGrounded() && isJumping)
        {
            state = PlayerState.OffGround;
        }

        if(!isGrounded() && !isJumping)
        {
            state = PlayerState.Fall;
        }

        switch(state)
        {
            case PlayerState.OnGround:
                coyoteTimeCouter = coyoteTime;
                Jumping();
                
                break;
            case PlayerState.OffGround:
                JumpingCoyote();
                break;
            case PlayerState.Fall:
                JumpingCoyote();
                break;
        }






    }

    private void FixedUpdate()
    {
        if(x != 0)
        {
            Move();
        }
    }

    private bool isGrounded()
    {
        bool result = (Physics2D.OverlapCircle(playerFoot.transform.position, 0.2f, groundLayer)) ? true : false;
        return result;
    }

    private void Move()
    {
        if(isGrounded())
        {
            rb.velocity = new Vector2(x * playerSpeed * Time.deltaTime, rb.velocity.y);
        }
        
        if(!isGrounded() && isJumping) 
        {
            rb.velocity = new Vector2(x * airSpeed * Time.deltaTime, rb.velocity.y);
        }
        
    }

    private void Jumping()
    {
        if (coyoteTimeCouter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, playerJumpPower);
            jumpBufferCounter = 0f;
            StartCoroutine(JumpCooldown());
        }
    }

    private void JumpingCoyote()
    {
        if (Input.GetButtonDown("Vertical"))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if (Input.GetButtonUp("Vertical") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCouter = 0f;
        }
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        rb.gravityScale = peakGravity;
        yield return new WaitForSeconds(0.4f);
        rb.gravityScale = normalGravity;
        isJumping = false;
    }

}
