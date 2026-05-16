using UnityEngine;

public class HealthHUDConnector : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;

    [Header("HUD")]
    [SerializeField] private LivesHUD livesHUD;

    private void Awake()
    {
        ResolveReferences();
    }

    private void OnEnable()
    {
        ResolveReferences();

        if (livesHUD == null)
            return;

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
        ResolveReferences();

        if (livesHUD == null)
            return;

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
        if (livesHUD == null)
            return;

        if (player1Health != null)
        {
            player1Health.OnLivesChanged -= livesHUD.UpdatePlayer1Lives;
        }

        if (player2Health != null)
        {
            player2Health.OnLivesChanged -= livesHUD.UpdatePlayer2Lives;
        }
    }

    private void ResolveReferences()
    {
        if (livesHUD == null)
            livesHUD = GetComponent<LivesHUD>();

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
