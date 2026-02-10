using UnityEngine;
using UnityEngine.Rendering;                 // Required for Volumes
using UnityEngine.Rendering.Universal;       // Required for URP Vignette
using System.Collections;

public class VignetteFader : MonoBehaviour
{
    public Volume globalVolume;              // Drag your Volume here
    public float fadeDuration = 3f;        // How fast it fades

    private Vignette _vignette;

    void Start()
    {
        // 1. Find the Vignette setting inside the volume
        if (globalVolume.profile.TryGet(out Vignette v))
        {
            _vignette = v;
        }
    }

    // Call this function when the player dies
    public void StartFadeToBlack()
    {
        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        float timer = 0;
        float startValue = _vignette.intensity.value;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            // Math to smoothly go from Current Value -> 1.0 (Full Black)
            float newIntensity = Mathf.Lerp(startValue, 1.0f, timer / fadeDuration);
            _vignette.intensity.value = newIntensity;

            yield return null; // Wait for next frame
        }

        // Ensure it is perfectly black at the end
        _vignette.intensity.value = 1.0f;
    }
}