using UnityEngine;

public class S_Menus : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        ResolveReferences();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();

        if (Input.GetKeyDown(KeyCode.Space) && gameManager != null && !gameManager.IsMatchStarted)
            Play();
    }

    public void Play()
    {
        HideMainMenu();

        if (gameManager != null)
            gameManager.StartMatch();
    }

    public void Quit()
    {
        Debug.Log("Saliendo de la aplicacion");
        Application.Quit();
    }

    private void ResolveReferences()
    {
        if (mainMenu == null)
            mainMenu = gameObject;

        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();
    }

    private void HideMainMenu()
    {
        if (mainMenu == null)
            return;

        if (mainMenu == gameObject)
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);

            return;
        }

        mainMenu.SetActive(false);
    }
}
