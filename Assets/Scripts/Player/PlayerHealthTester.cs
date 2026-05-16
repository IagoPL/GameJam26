//TODO Boprrar despues es un tester, esta referenciado en el objeto playerHealthtester en manager
using UnityEngine;

public class PlayerHealthTester : MonoBehaviour
{
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;

    private void Awake()
    {
        ResolveReferences();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (player1Health != null)
                player1Health.TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (player2Health != null)
                player2Health.TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (player1Health != null)
                player1Health.TakeDamage(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (player2Health != null)
                player2Health.TakeDamage(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (player1Health != null)
                player1Health.ResetLives();

            if (player2Health != null)
                player2Health.ResetLives();
        }
    }

    private void ResolveReferences()
    {
        if (player1Health == null)
            player1Health = FindPlayerHealthByTag("Player1");

        if (player2Health == null)
            player2Health = FindPlayerHealthByTag("Player2");
    }

    private PlayerHealth FindPlayerHealthByTag(string playerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        return player != null ? player.GetComponent<PlayerHealth>() : null;
    }
}
