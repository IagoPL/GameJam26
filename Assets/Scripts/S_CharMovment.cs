using UnityEngine;

public class S_CharMovment : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float speed = 5f;
    public float jumpForce = 7f;

    private bool isGrounded = false;
    private bool isBended = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
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
            
        }
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        UpdateAnimations(move);
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

        if (anim.GetBool("ifAttacking"))
            anim.SetBool("ifAttacking", false);
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
}