using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Match Settings")]
    [SerializeField] private float matchDuration = 90f;

    [Header("Players")]
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;

    [Header("UI")]
     [SerializeField] private MatchResultHUD matchResultHUD;

    private float currentTime;
    private bool matchStarted;
    private bool matchFinished;

    public float CurrentTime => currentTime;
    public bool IsMatchFinished => matchFinished;

    private void Awake()
    {
        ResolvePlayerReferences();
    }

    private void OnEnable()
    {
        ResolvePlayerReferences();

        if (player1Health != null)
            player1Health.OnPlayerDied += HandlePlayer1Died;

        if (player2Health != null)
            player2Health.OnPlayerDied += HandlePlayer2Died;
    }

    private void OnDisable()
    {
        if (player1Health != null)
            player1Health.OnPlayerDied -= HandlePlayer1Died;

        if (player2Health != null)
            player2Health.OnPlayerDied -= HandlePlayer2Died;
    }

    private void Start()
    {
        StartMatch();
    }

    private void Update()
    {
        if (!matchStarted || matchFinished)
            return;

        EndMatchIfAnyPlayerIsDead();

        if (matchFinished)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            KillRandomPlayerByTimeout();
        }
    }

    private void StartMatch()
    {
        currentTime = matchDuration;
        matchStarted = true;
        matchFinished = false;

        if (matchResultHUD != null)
            matchResultHUD.HideResult();

        Debug.Log("Partida iniciada");
        EndMatchIfAnyPlayerIsDead();
    }

    private void HandlePlayer1Died()
    {
        EndMatchWithWinner(2);
    }

    private void HandlePlayer2Died()
    {
        EndMatchWithWinner(1);
    }

    private void EndMatchWithWinner(int winnerPlayerNumber)
    {
        if (matchFinished)
            return;

        matchFinished = true;

        Debug.Log($"Gana Jugador {winnerPlayerNumber}");

        if (matchResultHUD != null)
            matchResultHUD.ShowWinner(winnerPlayerNumber);
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public bool HasMatchFinished()
    {
        return matchFinished;
    }

    private void KillRandomPlayerByTimeout()
    {
        if (matchFinished)
            return;

        if (player1Health == null || player2Health == null)
        {
            Debug.LogWarning("No se puede resolver el final por tiempo: falta PlayerHealth de algun jugador");
            return;
        }

        int randomPlayer = Random.Range(1, 3);

        if (randomPlayer == 1)
        {
            Debug.Log("Tiempo agotado: el Jugador 1 pierde todas sus vidas");
            player1Health.Kill();
        }
        else
        {
            Debug.Log("Tiempo agotado: el Jugador 2 pierde todas sus vidas");
            player2Health.Kill();
        }
    }

    private void EndMatchIfAnyPlayerIsDead()
    {
        if (matchFinished)
            return;

        if (player1Health != null && player1Health.IsDead)
        {
            EndMatchWithWinner(2);
            return;
        }

        if (player2Health != null && player2Health.IsDead)
        {
            EndMatchWithWinner(1);
        }
    }

    private void ResolvePlayerReferences()
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
