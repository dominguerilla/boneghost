using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When started, plays a set of sounds one at a time with a specified interval in between each sound.
/// Must be stopped to stop the sounds.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class RepetitiveSoundPlayer : MonoBehaviour
{
    public float interval = 0.2f;
    public AudioClip[] sounds;

    Coroutine playRoutine;
    int currentIndex = 0;
    [SerializeField] AudioSource audioSrc;

    private void Awake()
    {
        if(!audioSrc) audioSrc = GetComponent<AudioSource>();
    }

    public void StartPlaying()
    {
        playRoutine = StartCoroutine(SoundPlayRoutine(interval));
    }

    public void StopPlaying()
    {
        if(playRoutine != null) StopCoroutine(playRoutine);
    }

    IEnumerator SoundPlayRoutine(float interval)
    {
        currentIndex = 0;
        while (true)
        {
            audioSrc.clip = sounds[currentIndex];
            audioSrc.Play();
            yield return new WaitForSeconds(interval);
            currentIndex = (currentIndex + 1) % sounds.Length;
        }
    }
    
}
