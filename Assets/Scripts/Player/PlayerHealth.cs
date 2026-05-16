using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxLives = 5;
    [SerializeField] private float damageLockDuration = 1f;

    [Header("Debug")]
    [SerializeField] private int currentLives;
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;
    public event Action<int> OnLivesChanged;
    public event Action OnPlayerDied;

    public int CurrentLives => currentLives;
    public int MaxLives => maxLives;
    public bool IsDead => currentLives <= 0;
    public bool IsDamageLocked => Time.time < damageLockEndTime;

    private bool hasDied;
    private float damageLockEndTime;

    private void Awake()
    {
        currentLives = maxLives;
        hasDied = false;
        damageLockEndTime = 0f;
    }

    private void Start()
    {
        NotifyLivesChanged();
    }
    public void Kill()
    {
        if (hasDied)
            return;

        currentLives = 0;
        NotifyLivesChanged();
        Die();
    }

    public bool TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} recibe intento de dano: {damage}. Vidas antes: {currentLives}");
        if (hasDied)
        {
            Debug.Log($"{gameObject.name} ignora el dano porque ya esta muerto");
            return false;
        }

        if (IsDamageLocked)
        {
            Debug.Log($"{gameObject.name} ignora el dano porque esta en invulnerabilidad temporal");
            return false;
        }

        if (damage <= 0)
        {
            Debug.Log($"{gameObject.name} ignora el dano porque el valor no es positivo: {damage}");
            return false;
        }

        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        damageLockEndTime = Time.time + damageLockDuration;

        Debug.Log($"{gameObject.name} vidas despues del dano: {currentLives}");

        NotifyLivesChanged();

        if (currentLives <= 0)
        {
            Die();
        }

        return true;
    }

    public void Heal(int amount)
    {
        if (hasDied)
            return;

        if (amount <= 0)
            return;

        currentLives += amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        NotifyLivesChanged();
    }

    public void ResetLives()
    {
        currentLives = maxLives;
        hasDied = false;
        damageLockEndTime = 0f;
        NotifyLivesChanged();
    }

    private void Die()
    {
        if (hasDied)
            return;

        hasDied = true;
        Debug.Log($"{gameObject.name} ha muerto");
        OnPlayerDied?.Invoke();
    }

    private void NotifyLivesChanged()
    {
        OnLivesChanged?.Invoke(currentLives);
    }
}
