using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes
using TMPro; 

public class StorySceneController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameControls");
    }

    
}