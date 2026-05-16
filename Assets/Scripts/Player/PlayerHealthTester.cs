//TODO Boprrar despues es un tester, esta referenciado en el objeto playerHealthtester en manager
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

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player1Health.TakeDamage(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            player2Health.TakeDamage(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            player1Health.ResetLives();
            player2Health.ResetLives();
        }
    }
}