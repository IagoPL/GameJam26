using UnityEngine;
using UnityEngine.Events;

public class PlayerLives : MonoBehaviour
{
    [SerializeField] private int maxLives = 5;
    private int currentLives;
    private bool isDead;
    private bool isCriticDamage;

    public int CurrentLives => currentLives;
    public int MaxLives => maxLives;
    public bool IsDead => isDead;
    public bool IsCriticDamage => isCriticDamage;

    public UnityEvent<int> OnLivesChanged;
    public UnityEvent OnPlayerDied;

    void Start()
    {
        ResetLives();
    }

    public void TakeDamage()
    {
        if (isDead) return;

        if(isCriticDamage){
            currentLives = Mathf.Max(0, currentLives - 3);
        } else {
            currentLives = Mathf.Max(0, currentLives - 1);
        }
        
        OnLivesChanged?.Invoke(currentLives);

        if (currentLives <= 0)
        {
            isDead = true;
            OnPlayerDied?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        currentLives = Mathf.Min(maxLives, currentLives + amount);
        if (currentLives > 0) isDead = false;
        OnLivesChanged?.Invoke(currentLives);
    }

    public void ResetLives()
    {
        currentLives = maxLives;
        isDead = false;
        OnLivesChanged?.Invoke(currentLives);
    }
}
