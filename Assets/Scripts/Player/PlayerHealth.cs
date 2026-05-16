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
        Debug.Log($"{gameObject.name} recibe intento de dano: {damage}. Vidas antes: {currentLives}");
        if (hasDied)
        {
            Debug.Log($"{gameObject.name} ignora el dano porque ya esta muerto");
            return;
        }

        if (damage <= 0)
        {
            Debug.Log($"{gameObject.name} ignora el dano porque el valor no es positivo: {damage}");
            return;
        }

        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        Debug.Log($"{gameObject.name} vidas despues del dano: {currentLives}");

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
