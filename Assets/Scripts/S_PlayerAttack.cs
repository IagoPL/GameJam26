using UnityEngine;

public class S_PlayerAttack : MonoBehaviour
{
    
    [Header("Attack Necessities")] 
    [SerializeField] private GameObject attackBox;
    [SerializeField] public GameObject[] HitBoxPlayer1;
    [SerializeField] public GameObject[] HitBoxPlayer2;

    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.5f;

   
    private bool isAttacking = false;
    private string playerTag;

    private void Awake()
    {
        playerTag = gameObject.tag;
        attackBox.SetActive(false); 
    }

    private void Update()
    {
        if (playerTag == "Player1" && Input.GetKeyDown(KeyCode.F) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
        if (playerTag == "Player2" && Input.GetKeyDown(KeyCode.RightShift) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
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
