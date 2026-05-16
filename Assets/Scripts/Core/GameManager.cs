using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Match Settings")]
    [SerializeField] private float matchDuration = 60f;

    [Header("Players")]
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;

    [Header("Player Animators")]
    [SerializeField] private Animator player1Animator;
    [SerializeField] private Animator player2Animator;

    [Header("UI")]
     [SerializeField] private MatchResultHUD matchResultHUD;

    private float currentTime;
    private bool matchStarted;
    private bool matchFinished;
    private int pendingPlayer1WinAnimationValue;
    private int pendingPlayer2WinAnimationValue;

    public float CurrentTime => currentTime;
    public bool IsMatchFinished => matchFinished;

    private void Awake()
    {
        Instance = this;
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

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        StartMatch();
    }

    private void Update()
    {
        if (matchFinished)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                RestartMatch();

            return;
        }

        if (!matchStarted)
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
        ResolvePlayerReferences();

        currentTime = matchDuration;
        matchStarted = true;
        matchFinished = false;
        SetWinAnimations(0);

        if (matchResultHUD != null)
            matchResultHUD.HideResult();

        Debug.Log("Partida iniciada");
        EndMatchIfAnyPlayerIsDead();
    }

    private void HandlePlayer1Died()
    {
        EndMatchWithWinner(2, GetPlayer1WinAnimationValue(2), GetPlayer2WinAnimationValue(2));
    }

    private void HandlePlayer2Died()
    {
        EndMatchWithWinner(1, GetPlayer1WinAnimationValue(1), GetPlayer2WinAnimationValue(1));
    }

    private void EndMatchWithWinner(int winnerPlayerNumber)
    {
        EndMatchWithWinner(winnerPlayerNumber, winnerPlayerNumber, winnerPlayerNumber);
    }

    private void EndMatchWithWinner(int winnerPlayerNumber, int player1WinAnimationValue, int player2WinAnimationValue)
    {
        if (matchFinished)
            return;

        matchFinished = true;
        pendingPlayer1WinAnimationValue = 0;
        pendingPlayer2WinAnimationValue = 0;
        StopPlayerMovement();

        Debug.Log($"Gana Jugador {winnerPlayerNumber}");

        if (matchResultHUD != null)
            matchResultHUD.ShowWinner(winnerPlayerNumber);

        SetWinAnimations(player1WinAnimationValue, player2WinAnimationValue);
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
            pendingPlayer1WinAnimationValue = 3;
            pendingPlayer2WinAnimationValue = 2;
            player1Health.Kill();
        }
        else
        {
            Debug.Log("Tiempo agotado: el Jugador 2 pierde todas sus vidas");
            pendingPlayer1WinAnimationValue = 1;
            pendingPlayer2WinAnimationValue = 4;
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

        if (player1Animator == null && player1Health != null)
            player1Animator = player1Health.GetComponent<Animator>();

        if (player2Animator == null && player2Health != null)
            player2Animator = player2Health.GetComponent<Animator>();
    }

    private PlayerHealth FindPlayerHealthByTag(string playerTag)
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        return player != null ? player.GetComponent<PlayerHealth>() : null;
    }

    private int GetPlayer1WinAnimationValue(int winnerPlayerNumber)
    {
        if (pendingPlayer1WinAnimationValue != 0)
            return pendingPlayer1WinAnimationValue;

        return winnerPlayerNumber;
    }

    private int GetPlayer2WinAnimationValue(int winnerPlayerNumber)
    {
        if (pendingPlayer2WinAnimationValue != 0)
            return pendingPlayer2WinAnimationValue;

        return winnerPlayerNumber;
    }

    private void StopPlayerMovement()
    {
        StopPlayer(player1Health);
        StopPlayer(player2Health);
    }

    private void StopPlayer(PlayerHealth playerHealth)
    {
        if (playerHealth == null)
            return;

        Rigidbody2D rb = playerHealth.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    private void RestartMatch()
    {
        Debug.Log("Reiniciando partida");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetWinAnimations(int winnerPlayerNumber)
    {
        SetWinAnimations(winnerPlayerNumber, winnerPlayerNumber);
    }

    private void SetWinAnimations(int player1WinAnimationValue, int player2WinAnimationValue)
    {
        SetAnimatorWinValue(player1Animator, player1WinAnimationValue);
        SetAnimatorWinValue(player2Animator, player2WinAnimationValue);
    }

    private void SetAnimatorWinValue(Animator animator, int winnerPlayerNumber)
    {
        if (animator == null)
            return;

        if (HasAnimatorParameter(animator, "win", AnimatorControllerParameterType.Int))
            animator.SetInteger("win", winnerPlayerNumber);
    }

    private bool HasAnimatorParameter(Animator animator, string parameterName, AnimatorControllerParameterType parameterType)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name == parameterName && parameter.type == parameterType)
                return true;
        }

        return false;
    }
}
