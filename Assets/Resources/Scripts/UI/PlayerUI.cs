using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour {
    private bool pauseMenuActive;
    public GameObject pauseMenuBackground;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    private Scene activeScene;
    private PlayerHealth playerHealth;
    
    void Start() {
        pauseMenuActive = false;
        activeScene = SceneManager.GetActiveScene();

        playerHealth = FindObjectOfType<PlayerHealth>();
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !playerHealth.IsKill)
            ShowPauseMenu();

        if (playerHealth.IsKill && Time.timeScale != 0) {
            Time.timeScale = 0;
            pauseMenuBackground.SetActive(true);
            Cursor.visible = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void ShowPauseMenu() {
        pauseMenuActive = !pauseMenuActive;
        pauseMenuBackground.SetActive(pauseMenuActive);
        pauseMenu.SetActive(pauseMenuActive);
        Cursor.visible = pauseMenuActive;
        Time.timeScale = pauseMenuActive ? 0 : 1;
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(activeScene.name);
    }

    public void Continue() {
        ShowPauseMenu();
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
