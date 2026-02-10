using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes
using TMPro;
public class GameControlScene : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}