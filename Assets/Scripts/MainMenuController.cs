using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI soundButtonText; 

    private bool isSoundOn = true;

    void Start()
    {
        isSoundOn = true;
        AudioListener.volume = 1f;
        UpdateSoundText();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("StoryScene");
    }

    
    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;

        if (isSoundOn)
        {
            AudioListener.volume = 1f; // Unmute
        }
        else
        {
            AudioListener.volume = 0f; // Mute everything
        }

        UpdateSoundText();
    }

    void UpdateSoundText()
    {
        if (soundButtonText != null)
        {
            if (isSoundOn) soundButtonText.text = "SOUND: ON";
            else soundButtonText.text = "SOUND: OFF";
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
