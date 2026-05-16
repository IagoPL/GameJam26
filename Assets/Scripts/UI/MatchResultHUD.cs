using UnityEngine;
using TMPro;
public class MatchResultHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;

    public void ShowWinner(int winnerPlayerNumber)
    {
          Debug.Log("Mostrando ganador en HUD: Jugador " + winnerPlayerNumber);
        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (resultText != null)
            resultText.text = $"Gana Jugador {winnerPlayerNumber}";
    }

    public void ShowDraw()
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (resultText != null)
            resultText.text = "Empate";
    }

    public void HideResult()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);
    }
}