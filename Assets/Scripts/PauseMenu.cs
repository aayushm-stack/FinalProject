using UnityEngine;
using UnityEngine.SceneManagement; // Required for switching scenes

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI; // Drag your 'PauseMenu' panel here

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        // 1. Hide the Menu
        pauseMenuUI.SetActive(false);

        // 2. Unfreeze Time
        Time.timeScale = 1f;
        GameIsPaused = false;

        // 3. Lock Cursor (Hide it and lock to center)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        // 1. Show the Menu
        pauseMenuUI.SetActive(true);

        // 2. Freeze Time
        Time.timeScale = 0f;
        GameIsPaused = true;

        // 3. Unlock Cursor (Show it so you can click buttons)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMainMenu()
    {
        // IMPORTANT: Always reset time before leaving the scene!
        // If you don't, the Main Menu will be stuck in slow motion/frozen.
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Replace "MainMenu" with the EXACT name of your menu scene
        SceneManager.LoadScene("MainMenu");
    }

    //public void QuitGame()
    //{
    //    Debug.Log("Quitting Game...");
    //    Application.Quit();
    //}
}