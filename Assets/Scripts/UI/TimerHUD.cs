using TMPro;
using UnityEngine;

public class TimerHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text timerText;

    private void Update()
    {
        if (gameManager == null || timerText == null)
            return;

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        float currentTime = gameManager.GetCurrentTime();

        int totalSeconds = Mathf.CeilToInt(currentTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}