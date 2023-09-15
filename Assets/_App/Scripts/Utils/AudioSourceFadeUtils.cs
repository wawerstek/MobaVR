using System.Collections;
using UnityEngine;

namespace MobaVR.Utils
{
    public static class AudioSourceFadeUtils
    {
        /*
        public static IEnumerator FadeOut(AudioSource audioSource, float fadeOutVolume = 0f, float fadeTime = 1f)
        {
            float startVolume = audioSource.volume;
 
            while (audioSource.volume > fadeOutVolume)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
 
                yield return null;
            }
 
            audioSource.Stop();
            audioSource.volume = startVolume;
        }
 
        public static IEnumerator FadeIn(AudioSource audioSource, float fadeInVolume = 1.0f, float fadeTime = 1f)
        {
            float startVolume = 0.2f;
 
            audioSource.volume = 0;
            audioSource.Play();
 
            while (audioSource.volume < fadeInVolume)
            {
                audioSource.volume += startVolume * Time.deltaTime / fadeTime;
 
                yield return null;
            }
 
            audioSource.volume = fadeInVolume;
        }
        */
        
        /*public static void FadeOut(AudioSource audioSource, float fadeOutVolume = 0f, float fadeTime = 1f)
        {
            if (audioSource.TryGetComponent(out MonoBehaviour monoBehaviour))
            {
                monoBehaviour.StopCoroutine(nameof(FadeInCoroutine));
                monoBehaviour.StartCoroutine(FadeOutCoroutine(audioSource, fadeOutVolume, fadeTime));
            }
        }
        
        public static void FadeIn(AudioSource audioSource, float fadeOutVolume = 0f, float fadeTime = 1f)
        {
            if (audioSource.TryGetComponent(out MonoBehaviour monoBehaviour))
            {
                monoBehaviour.StopCoroutine(nameof(FadeOut));
                monoBehaviour.StartCoroutine(FadeInCoroutine(audioSource, fadeOutVolume, fadeTime));
            }
        }
 
        private static IEnumerator FadeOutCoroutine(AudioSource audioSource, float fadeOutVolume = 0f, float fadeTime = 1f)
        {
            float startVolume = audioSource.volume;
            float currentTime = 0f;
            while (audioSource.volume > fadeOutVolume || currentTime > fadeTime)
            {
                currentTime += Time.deltaTime;
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return new WaitForEndOfFrame();
            }
 
            audioSource.volume = fadeOutVolume;
            audioSource.Stop();
        }
        
        public static IEnumerator FadeInCoroutine(AudioSource audioSource, float fadeInVolume = 1.0f, float fadeTime = 1f)
        {
            float startVolume = audioSource.volume;
            float currentTime = 0f;

            audioSource.volume = startVolume;
            audioSource.Play();
 
            while (audioSource.volume < fadeInVolume || fadeTime > currentTime)
            {
                currentTime += Time.deltaTime;
                audioSource.volume += startVolume * Time.deltaTime / fadeTime; //TODO
                yield return null;
            }
 
            audioSource.volume = fadeInVolume;
        }*/
    }
}