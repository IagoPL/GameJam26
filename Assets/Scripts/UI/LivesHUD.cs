using UnityEngine;
using UnityEngine.UI;

public class LivesHUD : MonoBehaviour
{
    [Header("Player 1 Lives")]
    [SerializeField] private Image[] player1LifeImages;

    [Header("Player 2 Lives")]
    [SerializeField] private Image[] player2LifeImages;

    [Header("Sprites")]
    [SerializeField] private Sprite player1FullHeartSprite;
    [SerializeField] private Sprite player2FullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    private const int MaxLives = 5;

    private void Start()
    {
        UpdatePlayer1Lives(MaxLives);
        UpdatePlayer2Lives(MaxLives);
    }

    public void UpdatePlayer1Lives(int currentLives)
    {
        UpdateLives(player1LifeImages, currentLives, player1FullHeartSprite);
    }

    public void UpdatePlayer2Lives(int currentLives)
    {
        UpdateLives(player2LifeImages, currentLives, player2FullHeartSprite);
    }

    private void UpdateLives(Image[] lifeImages, int currentLives, Sprite fullHeartSprite)
    {
        int clampedLives = Mathf.Clamp(currentLives, 0, MaxLives);

        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (lifeImages[i] == null)
                continue;

            bool hasLife = i < clampedLives;
            lifeImages[i].sprite = hasLife ? fullHeartSprite : emptyHeartSprite;
            lifeImages[i].preserveAspect = true;
        }
    }
}