using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalSceneController : MonoBehaviour
{
    
    public void CloseApplication()
    {
        Debug.Log("Exit Game Requested. Quitting...");

        
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}