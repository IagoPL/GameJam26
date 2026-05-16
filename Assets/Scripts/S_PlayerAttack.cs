using UnityEngine;

public class S_PlayerAttack : MonoBehaviour
{
    
    [Header("Attack Necessities")] 
    [SerializeField] private GameObject attackBox;
    [SerializeField] public GameObject[] HitBoxPlayer1;
    [SerializeField] public GameObject[] HitBoxPlayer2;

    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.5f;

    [Header("Attack Positions")]
    private Vector3 idleAttackOffset = new Vector3(0.1f, 0.13f, 0f);
    [SerializeField] private Vector3 crouchAttackOffset = new Vector3(0.1f, 0.05f, 0f);
    [SerializeField] private Vector3 jumpAttackOffset = new Vector3(0.15f, 0.25f, 0f);

    private bool isAttacking = false;
    private string playerTag;

    public bool isBended = false;
    public bool isGrounded = true;
    public bool facingRight = true;

    private void Awake()
    {
        playerTag = gameObject.tag;
        attackBox.SetActive(false); 
    }

    private void Update()
    {
        if (playerTag == "Player1" && Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            SetAttackBoxPosition();
            StartCoroutine(PerformAttack());
        }
        if (playerTag == "Player2" && Input.GetKeyDown(KeyCode.RightShift) && !isAttacking)
        {
            SetAttackBoxPosition();
            StartCoroutine(PerformAttack());
        }
    }

     private void SetAttackBoxPosition()
    {
         Vector3 offset = idleAttackOffset;
        if (isBended)
        {
            offset = crouchAttackOffset; 
        }
        else if (!isGrounded)
        {
            offset = jumpAttackOffset;
        }

        offset.x = offset.x * (facingRight ? 1f : -1f);
        attackBox.transform.localPosition = offset;
    }

    
     private System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;
        attackBox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attackBox.SetActive(false); 
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAttacking) return; 

        if (playerTag == "Player1" && collision.CompareTag("HitBoxPlayer2"))
        {
            Debug.Log("Player2 recibe daño!");
        }

        if (playerTag == "Player2" && collision.CompareTag("HitBoxPlayer1"))
        {
            Debug.Log("Player1 recibe daño!");
        }
    }
    
}
