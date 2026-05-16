using UnityEngine;

public class HealthHUDConnector : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;

    [Header("HUD")]
    [SerializeField] private LivesHUD livesHUD;

    private void OnEnable()
    {
        if (player1Health != null)
        {
            player1Health.OnLivesChanged += livesHUD.UpdatePlayer1Lives;
        }

        if (player2Health != null)
        {
            player2Health.OnLivesChanged += livesHUD.UpdatePlayer2Lives;
        }
    }

    private void Start()
    {
        if (player1Health != null)
        {
            livesHUD.UpdatePlayer1Lives(player1Health.CurrentLives);
        }

        if (player2Health != null)
        {
            livesHUD.UpdatePlayer2Lives(player2Health.CurrentLives);
        }
    }

    private void OnDisable()
    {
        if (player1Health != null)
        {
            player1Health.OnLivesChanged -= livesHUD.UpdatePlayer1Lives;
        }

        if (player2Health != null)
        {
            player2Health.OnLivesChanged -= livesHUD.UpdatePlayer2Lives;
        }
    }
}