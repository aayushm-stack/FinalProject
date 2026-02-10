using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class missionFail : MonoBehaviour
{
    
    
    public void LoadMenu()
    {
        Debug.Log("Switching to Main Menu ...");
        SceneManager.LoadScene("MainMenu");
    }
}