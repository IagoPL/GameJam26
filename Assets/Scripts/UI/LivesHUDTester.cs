using UnityEngine;

public class LivesHUDTester : MonoBehaviour
{
    [SerializeField] private LivesHUD livesHUD;

    private int player1Lives = 5;
    private int player2Lives = 5;

    private void Start()
    {
        livesHUD.UpdatePlayer1Lives(player1Lives);
        livesHUD.UpdatePlayer2Lives(player2Lives);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            player1Lives = Mathf.Max(0, player1Lives - 1);
            livesHUD.UpdatePlayer1Lives(player1Lives);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            player2Lives = Mathf.Max(0, player2Lives - 1);
            livesHUD.UpdatePlayer2Lives(player2Lives);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player1Lives = 5;
            player2Lives = 5;

            livesHUD.UpdatePlayer1Lives(player1Lives);
            livesHUD.UpdatePlayer2Lives(player2Lives);
        }
    }
}