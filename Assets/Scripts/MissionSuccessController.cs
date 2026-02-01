using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionSuccessController : MonoBehaviour
{
    [Header("ReferenceAudios")]
    public AudioSource voiceOverSource;
    async void Start()
    {
        voiceOverSource.Play();
        await Task.Delay(300);  //second way for delay
        voiceOverSource.Play();

    }
    public void CloseApplication()
    {
        Debug.Log("Exit Game Requested. Quitting...");

        
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}