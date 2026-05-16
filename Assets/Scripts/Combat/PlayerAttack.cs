using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private KeyCode attackKey = KeyCode.F;

    [Header("Combat")]
    [SerializeField] private float cooldownDuration = 0.4f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private Transform attackPoint;

    [Header("Headshot")]
    [SerializeField][Range(0f, 1f)] private float headshotZone = 0.3f;

    private float lastAttackTime;
    private PlayerLives playerLives;

    private bool CanAttack => Time.time >= lastAttackTime + cooldownDuration;

    void Awake()
    {
        playerLives = GetComponent<PlayerLives>();
        if (attackPoint == null) attackPoint = transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(attackKey) && CanAttack)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        float facing = Mathf.Sign(transform.localScale.x);
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            Vector2 dirToTarget = (hit.bounds.center - (Vector2)attackPoint.position).normalized;
            if (dirToTarget.x * facing < 0f) continue;

            PlayerLives opponent = hit.GetComponentInParent<PlayerLives>();
            if (opponent != null && opponent != playerLives && !opponent.IsDead)
            {
                bool isHeadshot = IsHeadshot(hit);
                opponent.TakeDamage(isHeadshot);
                return;
            }
        }
    }

    bool IsHeadshot(Collider2D hit)
    {
        float headThreshold = hit.bounds.max.y - hit.bounds.size.y * (1f - headshotZone);
        return attackPoint.position.y >= headThreshold;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
