using UnityEngine;
using TMPro;
public class MatchResultHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject Player1Win;
    [SerializeField] private GameObject Player2Win;


    public void ShowWinner(int winnerPlayerNumber)
    {
          Debug.Log("Mostrando ganador en HUD: Jugador " + winnerPlayerNumber);
        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (Player1Win != null && Player2Win != null)
        {
            if (winnerPlayerNumber == 1)
            {
                Player1Win.SetActive (true);
                Player2Win.SetActive (false);
            }
            else
            {
                Player1Win.SetActive (false);
                Player2Win.SetActive (true);
            }
        }
            
    }

    public void ShowDraw()
    {
        if (resultPanel != null)
            resultPanel.SetActive(true);
    }

    public void HideResult()
    {
        if (resultPanel != null)
            resultPanel.SetActive(false);
    }
}