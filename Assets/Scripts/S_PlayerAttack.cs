using UnityEngine;
using System.Collections;

public class S_PlayerAttack : MonoBehaviour
{
    [Header("Attack Necessities")]
    [SerializeField] private GameObject attackBox;

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

    private IEnumerator PerformAttack()
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

        PlayerHealth targetHealth = collision.GetComponent<PlayerHealth>();
        if (targetHealth != null && collision.gameObject != gameObject)
        {
            targetHealth.TakeDamage(1);
        }
    }
}