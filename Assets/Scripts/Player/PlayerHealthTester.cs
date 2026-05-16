using UnityEngine;

public class PlayerHealthTester : MonoBehaviour
{
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player1Health.TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player2Health.TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            player1Health.TakeDamage(2);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            player2Health.TakeDamage(2);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player1Health.ResetLives();
            player2Health.ResetLives();
        }
    }
}