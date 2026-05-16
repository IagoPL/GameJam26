using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Match Settings")]
    [SerializeField] private float matchDuration = 90f;

    private float currentTime;
    private bool matchStarted;
    private bool matchFinished;

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

        Debug.Log("Partida iniciada");
    }

    private void EndMatchAsDraw()
    {
        matchFinished = true;
        Debug.Log("Empate por tiempo");
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public bool IsMatchFinished()
    {
        return matchFinished;
    }
}