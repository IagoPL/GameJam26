using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_PlayerAttack : MonoBehaviour
{
    [Header("Attack Necessities")]
    [SerializeField] private GameObject attackBox;

    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] private int normalDamage = 1;
    [SerializeField] private int criticalDamage = 2;
    [SerializeField, Range(0f, 1f)] private float criticalChance = 0.15f;

    [Header("Knockback")]
    [SerializeField] private float attackerKnockbackForce = 2f;
    [SerializeField] private float targetKnockbackForce = 6f;
    [SerializeField] private float knockbackUpForce = 1f;
    [SerializeField] private float knockbackDuration = 0.2f;

    private bool isAttacking = false;
    private string playerTag;
    private PlayerHealth playerHealth;
    private Rigidbody2D rb;
    private Coroutine attackCoroutine;
    private readonly HashSet<PlayerHealth> damagedTargets = new HashSet<PlayerHealth>();

    private void Awake()
    {
        playerTag = gameObject.tag;
        playerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();

        if (attackBox != null)
        {
            Debug.Log($"{gameObject.name} ataque inicializado con attackBox: {attackBox.name}");
            attackBox.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} no tiene attackBox asignado en S_PlayerAttack");
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsMatchFinished)
        {
            if (isAttacking)
                CancelAttack();

            return;
        }

        if (isAttacking && playerHealth != null && playerHealth.IsDamageLocked)
        {
            CancelAttackBecauseDamageWasReceived();
            return;
        }

        if (playerTag == "Player1" && Input.GetKeyDown(KeyCode.F) && CanStartAttack())
        {
            attackCoroutine = StartCoroutine(PerformAttack());
        }

        if (playerTag == "Player2" && Input.GetKeyDown(KeyCode.RightShift) && CanStartAttack())
        {
            attackCoroutine = StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        if (attackBox == null)
        {
            Debug.LogWarning($"{gameObject.name} intenta atacar, pero attackBox es null");
            yield break;
        }

        isAttacking = true;
        damagedTargets.Clear();
        Debug.Log($"{gameObject.name} empieza ataque. Activando collider: {attackBox.name}");
        attackBox.SetActive(true); 
        yield return new WaitForSeconds(attackDuration);
        attackBox.SetActive(false); 
        isAttacking = false;
        damagedTargets.Clear();
        attackCoroutine = null;
        Debug.Log($"{gameObject.name} termina ataque. Desactivando collider: {attackBox.name}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{gameObject.name} OnTriggerEnter2D con {collision.gameObject.name}. Atacando: {isAttacking}");
        TryDamageTarget(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log($"{gameObject.name} OnTriggerStay2D con {collision.gameObject.name}. Atacando: {isAttacking}");
        TryDamageTarget(collision);
    }

    private void TryDamageTarget(Collider2D collision)
    {
        if (!isAttacking)
        {
            Debug.Log($"{gameObject.name} detecta {collision.gameObject.name}, pero no esta atacando");
            return;
        }

        PlayerHealth targetHealth = collision.GetComponentInParent<PlayerHealth>();
        if (targetHealth == null)
        {
            Debug.Log($"{gameObject.name} detecta {collision.gameObject.name}, pero no encontro PlayerHealth en el objeto ni en sus padres");
            return;
        }

        if (targetHealth.gameObject == gameObject)
        {
            Debug.Log($"{gameObject.name} ignora su propio collider: {collision.gameObject.name}");
            return;
        }

        if (!damagedTargets.Add(targetHealth))
        {
            Debug.Log($"{gameObject.name} ya habia golpeado a {targetHealth.gameObject.name} en este ataque");
            return;
        }

        bool isCritical = Random.value < criticalChance;
        int damage = isCritical ? criticalDamage : normalDamage;

        Debug.Log($"{gameObject.name} intenta aplicar {damage} de dano a {targetHealth.gameObject.name} por collider {collision.gameObject.name}. Critico: {isCritical}");

        if (targetHealth.TakeDamage(damage))
        {
            ApplyKnockback(targetHealth);
            Debug.Log($"{gameObject.name} golpe aplicado a {targetHealth.gameObject.name}. Dano final: {damage}");
        }
        else
        {
            Debug.Log($"{gameObject.name} golpe bloqueado por {targetHealth.gameObject.name}");
        }
    }

    private bool CanStartAttack()
    {
        if (isAttacking)
            return false;

        if (playerHealth != null && playerHealth.IsDamageLocked)
        {
            Debug.Log($"{gameObject.name} no puede atacar porque acaba de recibir dano");
            return false;
        }

        return true;
    }

    private void CancelAttackBecauseDamageWasReceived()
    {
        CancelAttack();
        Debug.Log($"{gameObject.name} cancela su ataque porque acaba de recibir dano");
    }

    private void CancelAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        if (attackBox != null)
            attackBox.SetActive(false);

        isAttacking = false;
        damagedTargets.Clear();
    }

    private void ApplyKnockback(PlayerHealth targetHealth)
    {
        Rigidbody2D targetRb = targetHealth.GetComponent<Rigidbody2D>();
        Vector2 direction = ((Vector2)targetHealth.transform.position - (Vector2)transform.position).normalized;

        if (Mathf.Abs(direction.x) < 0.01f)
            direction.x = transform.localScale.x >= 0f ? 1f : -1f;

        direction.y = 0f;
        direction.Normalize();

        Vector2 targetVelocity = direction * targetKnockbackForce + Vector2.up * knockbackUpForce;
        Vector2 attackerVelocity = -direction * attackerKnockbackForce + Vector2.up * (knockbackUpForce * 0.5f);

        if (targetRb != null)
        {
            S_CharMovment targetMovement = targetHealth.GetComponent<S_CharMovment>();
            if (targetMovement != null)
                targetMovement.ApplyKnockback(targetVelocity, knockbackDuration);
            else
                targetRb.linearVelocity = new Vector2(targetVelocity.x, Mathf.Max(targetRb.linearVelocity.y, targetVelocity.y));

            Debug.Log($"{targetHealth.gameObject.name} recibe knockback: {targetVelocity}");
        }

        if (rb != null)
        {
            S_CharMovment attackerMovement = GetComponent<S_CharMovment>();
            if (attackerMovement != null)
                attackerMovement.ApplyKnockback(attackerVelocity, knockbackDuration);
            else
                rb.linearVelocity = new Vector2(attackerVelocity.x, Mathf.Max(rb.linearVelocity.y, attackerVelocity.y));

            Debug.Log($"{gameObject.name} retrocede por impacto: {attackerVelocity}");
        }
    }
}
