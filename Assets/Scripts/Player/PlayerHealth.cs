using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxLives = 5;

    [Header("Debug")]
    [SerializeField] private int currentLives;
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;
    public event Action<int> OnLivesChanged;
    public event Action OnPlayerDied;

    public int CurrentLives => currentLives;
    public int MaxLives => maxLives;
    public bool IsDead => currentLives <= 0;

    private bool hasDied;

    private void Awake()
    {
        currentLives = maxLives;
        hasDied = false;
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

    public void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} recibe {damage} de daño");
        if (hasDied)
            return;

        if (damage <= 0)
            return;

        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        NotifyLivesChanged();

        if (currentLives <= 0)
        {
            Die();
        }
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