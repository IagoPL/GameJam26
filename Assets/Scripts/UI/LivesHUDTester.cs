using UnityEngine;

public class LivesHUDTester : MonoBehaviour
{
    [SerializeField] private LivesHUD livesHUD;
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;

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
            player1Health.OnLivesChanged += livesHUD.UpdatePlayer1Lives;

        if (player2Health != null)
            player2Health.OnLivesChanged += livesHUD.UpdatePlayer2Lives;
    }

    private void Start()
    {
        RefreshHUD();
    }

    private void OnDisable()
    {
        if (livesHUD == null)
            return;

        if (player1Health != null)
            player1Health.OnLivesChanged -= livesHUD.UpdatePlayer1Lives;

        if (player2Health != null)
            player2Health.OnLivesChanged -= livesHUD.UpdatePlayer2Lives;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (player1Health != null)
                player1Health.TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (player2Health != null)
                player2Health.TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (player1Health != null)
                player1Health.ResetLives();

            if (player2Health != null)
                player2Health.ResetLives();
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

    private void RefreshHUD()
    {
        if (livesHUD == null)
            return;

        if (player1Health != null)
            livesHUD.UpdatePlayer1Lives(player1Health.CurrentLives);

        if (player2Health != null)
            livesHUD.UpdatePlayer2Lives(player2Health.CurrentLives);
    }

    private PlayerHealth FindPlayerHealthByTag(string playerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        return player != null ? player.GetComponent<PlayerHealth>() : null;
    }
}
