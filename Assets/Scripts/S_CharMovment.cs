using UnityEngine;

public class S_CharMovment : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float speed = 5f;
    public float jumpForce = 7f;

    private bool isGrounded = false;
    private bool isBended = false;
    private bool facingRight = true; 
    private float crouchColliderHeight; 

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    private BoxCollider2D boxCollider;
    private float originalColliderHeight;
    private Vector2 originalColliderOffset;
    private Vector2 knockbackVelocity;
    private float knockbackEndTime;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();

        boxCollider = GetComponent<BoxCollider2D>();
        originalColliderHeight = boxCollider.size.y;
        originalColliderOffset = boxCollider.offset;

        crouchColliderHeight = originalColliderHeight * 0.6f;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Time.time < knockbackEndTime)
        {
            rb.linearVelocity = new Vector2(knockbackVelocity.x, Mathf.Max(rb.linearVelocity.y, knockbackVelocity.y));
            UpdateAnimations(0f);
            HandleBending();
            return;
        }

        float move = 0f;
        if (CompareTag("Player1")) //WASD
        {
            
            if (Input.GetKey(KeyCode.A)) move = -1f;
            else if (Input.GetKey(KeyCode.D)) move = 1f;

            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            if (Input.GetKey(KeyCode.S))
            {
                isBended = true;
            }
            else {isBended = false;}

            if (Input.GetKeyDown(KeyCode.E))
            {
                FlipSprite();
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                anim.SetTrigger("Attack"); 
            }
        }
        if (CompareTag("Player2")) //Arrows
        {
            if (Input.GetKey(KeyCode.LeftArrow)) move = -1f;
            else if (Input.GetKey(KeyCode.RightArrow)) move = 1f;

            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                isBended = true;
            }
            else {isBended = false;}
            if (Input.GetKeyDown(KeyCode.M))
            {
               FlipSprite();
            }
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                anim.SetTrigger("Attack");
            }
        }
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        UpdateAnimations(move);
        HandleBending();
    }

    private void UpdateAnimations(float move)
    {
        if (Mathf.Abs(move) > 0.1f && isGrounded)
            anim.SetBool("ifWalking", true);
        else
            anim.SetBool("ifWalking", false);

        if (!isGrounded)
            anim.SetBool("ifJumping", true);
        else
            anim.SetBool("ifJumping", false);

        if (isBended == true)
            anim.SetBool("ifBending", true);
        else if (isBended == false)   
            anim.SetBool("ifBending", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
    private void FlipSprite()
    {
        facingRight = !facingRight; 
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void HandleBending()
    {
       if (isBended)
    {
        float deltaHeight = originalColliderHeight - crouchColliderHeight;
        boxCollider.size = new Vector2(boxCollider.size.x, crouchColliderHeight);
        boxCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - deltaHeight / 2f);
    }
    else
    {
        boxCollider.size = new Vector2(boxCollider.size.x, originalColliderHeight);
        boxCollider.offset = originalColliderOffset;
    }
    }

    public void ApplyKnockback(Vector2 velocity, float duration)
    {
        knockbackVelocity = velocity;
        knockbackEndTime = Time.time + duration;
    }
}
