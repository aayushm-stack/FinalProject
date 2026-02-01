using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; 

public class LastSceneControl
    : MonoBehaviour
{
    
   
    public float delayAudio = 2.0f;  // Gap for audio

    void Start()
    {
        StartCoroutine(PlaySequence()); //first way for delay
    }

    IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(delayAudio);
      
        SceneManager.LoadScene("MissionSuccess");
    }
}