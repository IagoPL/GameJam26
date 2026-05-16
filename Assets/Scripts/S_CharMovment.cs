using UnityEngine;

public class S_CharMovment : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    public float jumpForce = 7f;

    private float move = 0f;
    private bool isGrounded = false;

    private void Awake()
    {
        rb = GetComponent <Rigidbody2D>();
        rb.freezeRotation = true;
    }

    public void Update()
    {
        if (rb.GetComponent<Rigidbody2D>() == null)
            return;
        
        MovementCharacter();
        
    }
    private void MovementCharacter ()
    {
        float move = 0f;
        if (CompareTag("Player1"))
        {
            if (Input.GetKey(KeyCode.A)) move = -1f;
            if (Input.GetKey(KeyCode.D)) move = 1f;

            if (Input.GetKeyDown(KeyCode.W) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
        if (CompareTag("Player2"))
        {
            if (Input.GetKey(KeyCode.LeftArrow)) move = -1f;
            if (Input.GetKey(KeyCode.RightArrow)) move = 1f;

            if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }

        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
