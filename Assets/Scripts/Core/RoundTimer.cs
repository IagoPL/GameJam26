using UnityEngine;
using UnityEngine.Events;

public class RoundTimer : MonoBehaviour
{
    [SerializeField] private float startTime = 90f;
    private float currentTime;
    private bool isRunning;

    public float CurrentTime => currentTime;
    public float StartTime => startTime;
    public bool IsRunning => isRunning;

    public UnityEvent OnTimeRanOut;
    public UnityEvent<float> OnTick;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            OnTick?.Invoke(currentTime);
            OnTimeRanOut?.Invoke();
        }
        else
        {
            OnTick?.Invoke(currentTime);
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        isRunning = true;
        OnTick?.Invoke(currentTime);
    }
}
