using UnityEngine;
using UnityEngine.UI;

public class LivesHUD : MonoBehaviour
{
    [Header("Player 1 Lives")]
    [SerializeField] private Image[] player1LifeBoxes;

    [Header("Player 2 Lives")]
    [SerializeField] private Image[] player2LifeBoxes;

    [Header("Colors")]
    [SerializeField] private Color activeLifeColor = Color.white;
    [SerializeField] private Color inactiveLifeColor = new Color(1f, 1f, 1f, 0.2f);

    public void UpdatePlayer1Lives(int currentLives)
    {
        UpdateLives(player1LifeBoxes, currentLives);
    }

    public void UpdatePlayer2Lives(int currentLives)
    {
        UpdateLives(player2LifeBoxes, currentLives);
    }

    private void UpdateLives(Image[] lifeBoxes, int currentLives)
    {
        for (int i = 0; i < lifeBoxes.Length; i++)
        {
            if (lifeBoxes[i] == null)
                continue;

            bool isActive = i < currentLives;
            lifeBoxes[i].color = isActive ? activeLifeColor : inactiveLifeColor;
        }
    }
}