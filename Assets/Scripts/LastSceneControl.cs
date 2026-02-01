using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro; 

public class LastSceneControl
    : MonoBehaviour
{
    [Header("Settings")]
   
    public float delayAudio = 2.0f;  // Gap for audio

    [Header("ReferenceAudios")]
    public AudioSource voiceOverSource;
    

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(delayAudio);
        voiceOverSource.Play();
        yield return new WaitForSeconds(0.3f);
        voiceOverSource.Play();
       
        yield return new WaitForSeconds(voiceOverSource.clip.length+ delayAudio);
      
        SceneManager.LoadScene("MissionSuccess");
    }
}