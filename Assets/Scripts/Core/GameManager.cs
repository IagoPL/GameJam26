using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Match Settings")]
    [SerializeField] private float matchDuration = 90f;

    [Header("Players")]
    [SerializeField] private PlayerLives player1Lives;
    [SerializeField] private PlayerLives player2Lives;

    private float currentTime;
    private bool matchStarted;
    private bool matchFinished;

    private void Awake()
    {
        if (player1Lives == null || player2Lives == null)
        {
            PlayerLives[] players = FindObjectsByType<PlayerLives>(FindObjectsSortMode.None);
            foreach (var p in players)
            {
                if (p.gameObject.name == "Player1")
                    player1Lives = p;
                else if (p.gameObject.name == "Player2")
                    player2Lives = p;
            }
        }
    }

    private void Start()
    {
        StartMatch();
    }

    private void Update()
    {
        if (!matchStarted || matchFinished)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            EndMatchAsDraw();
        }
    }

    private void StartMatch()
    {
        currentTime = matchDuration;
        matchStarted = true;
        matchFinished = false;

        player1Lives.OnPlayerDied.AddListener(() => EndMatchWithWinner(2));
        player2Lives.OnPlayerDied.AddListener(() => EndMatchWithWinner(1));

        Debug.Log("Partida iniciada");
    }

    private void EndMatchAsDraw()
    {
        if (matchFinished) return;
        matchFinished = true;
        Debug.Log("Empate por tiempo");
    }

    private void EndMatchWithWinner(int winnerPlayerNumber)
    {
        if (matchFinished) return;
        matchFinished = true;
        Debug.Log($"Jugador {winnerPlayerNumber} gana");
    }

    public float GetCurrentTime() => currentTime;
    public bool IsMatchFinished() => matchFinished;
}
