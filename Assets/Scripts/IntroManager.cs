using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject cutsceneCamera;
    public GameObject mainCamera;

    [Header("Actors")]
    public GameObject cutscenePlayerModel; 
    public GameObject realPlayer;          

    [Header("Settings")]
    public float cutsceneDuration = 0.8f; 

    void Start()
    { 
        cutsceneCamera.SetActive(true);
        mainCamera.SetActive(false);

        realPlayer.SetActive(false);
       
        Invoke("SwitchToGameplay", cutsceneDuration);
    }

    void SwitchToGameplay()
    {
        // 1. Swap Cameras
        cutsceneCamera.SetActive(false);
        mainCamera.SetActive(true);
        realPlayer.SetActive(true);

        if (cutscenePlayerModel != null)
        {
            Destroy(cutscenePlayerModel);
            Destroy(cutsceneCamera);
        }
        Debug.Log("Cutscene Ended. Player has control.");
    }
}