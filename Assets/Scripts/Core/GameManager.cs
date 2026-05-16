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

    private void OnEnable()
    {
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

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
           /* EndMatchAsDraw() */    KillRandomPlayerByTimeout();
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

    private void EndMatchAsDraw() //TODO cambiar para que esplote uno al perder
    {
        if (matchFinished)
            return;

        matchFinished = true;

        Debug.Log("Empate por tiempo 'PD Mensaje temporal'");

        if (matchResultHUD != null)
            matchResultHUD.ShowDraw();
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

    int randomPlayer = Random.Range(1, 3); // 1 o 2

    if (randomPlayer == 1)
    {
        Debug.Log("Tiempo agotado: muere aleatoriamente el Jugador 1");
        player1Health.TakeDamage(999);
         EndMatchWithWinner(1);
    }
    else
    {
        Debug.Log("Tiempo agotado: muere aleatoriamente el Jugador 2");
        player2Health.TakeDamage(999);
        EndMatchWithWinner(2);
    }
}


}
