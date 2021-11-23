using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMenuUI : MonoBehaviour
{
    private bool pauseMenuActive;
    public GameObject pauseMenuBackground;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    private Scene activeScene;
    private PlayerHealth playerHealth;

    private GameManager gameManager;
    
    void Start() 
    {
        pauseMenuActive = false;
        activeScene = SceneManager.GetActiveScene();

        playerHealth = FindObjectOfType<PlayerHealth>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ShowPauseMenu();

        if (gameManager.PlayersLeft == 1 && Time.timeScale != 0)
        {
            Time.timeScale = 0;
            pauseMenuBackground.SetActive(true);
            Cursor.visible = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void ShowPauseMenu()
    {
        pauseMenuActive = !pauseMenuActive;
        pauseMenuBackground.SetActive(pauseMenuActive);
        pauseMenu.SetActive(pauseMenuActive);
        Cursor.visible = pauseMenuActive;
        Time.timeScale = pauseMenuActive ? 0 : 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(activeScene.name);
    }

    public void Continue()
    {
        ShowPauseMenu();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
