using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class S_PlayerAttack : MonoBehaviour
{
    [Header("Attack Necessities")]
    [SerializeField] private GameObject attackBox;

    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.5f;

    private bool isAttacking = false;
    private string playerTag;
    private readonly HashSet<PlayerHealth> damagedTargets = new HashSet<PlayerHealth>();

    private void Awake()
    {
        playerTag = gameObject.tag;
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

        Debug.Log($"{gameObject.name} aplica 1 de dano a {targetHealth.gameObject.name} por collider {collision.gameObject.name}");
        targetHealth.TakeDamage(1);
    }
}
