using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]

 
    public class AudioTrack
    {
        public string trackName;
        public AudioClip clip;

    }

    public List<AudioTrack> audioTracks; 
    public List<AudioTrack> sounds; 

    public AudioSource currentAudioSource; 
    private AudioSource newAudioSource;
    private AudioSource soundEffectSource; 

    public float fadeDuration = 1.0f; 
    public bool playing = false;

    void Awake()
    {
        if (currentAudioSource == null)
        {
            currentAudioSource = gameObject.AddComponent<AudioSource>();
            currentAudioSource.loop = true;
        }

        newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.loop = true;

        soundEffectSource = gameObject.AddComponent<AudioSource>();
        soundEffectSource.loop = false;
        Debug.Log("AudioManager initialized.");
    }

  

    // Play background music
    public void PlayTrack(string trackName)
    {
        AudioTrack selectedTrack = audioTracks.Find(track => track.trackName == trackName);
        if (selectedTrack == null)
        {
            Debug.LogError($"Track '{trackName}' not found!");
            return;
        }

        if (currentAudioSource.isPlaying)
        {
            StartCoroutine(CrossfadeToNewTrack(selectedTrack.clip));
        }
        else
        {
            StartCoroutine(FadeInTrack(selectedTrack.clip, currentAudioSource));
        }
    }

    private IEnumerator FadeInTrack(AudioClip clip, AudioSource audioSource)
    {
        audioSource.clip = clip;
        audioSource.volume = 0;
        audioSource.Play();

        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, 0.5f, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0.5f;
    }

    private IEnumerator CrossfadeToNewTrack(AudioClip newClip)
    {
        newAudioSource.clip = newClip;
        newAudioSource.volume = 0;
        newAudioSource.Play();

        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            currentAudioSource.volume = Mathf.Lerp(0.5f, 0, timer / fadeDuration);
            newAudioSource.volume = Mathf.Lerp(0, 0.5f, timer / fadeDuration);
            yield return null;
        }

        currentAudioSource.Stop();
        currentAudioSource.clip = null;

        // Swapping AudioSource
        var temp = currentAudioSource;
        currentAudioSource = newAudioSource;
        newAudioSource = temp;

        currentAudioSource.volume = 0.5f;
        newAudioSource.volume = 0;
    }

    // Play sound effects
    public void ManageSound(string soundName, bool play)
    {
        AudioTrack selectedSound = sounds.Find(sound => sound.trackName == soundName);
        if (selectedSound == null)
        {
            Debug.LogError($"Sound '{soundName}' not found in the sounds list!");
            return;
        }

        if (play)
        {
            Debug.Log($"Playing sound: {selectedSound.trackName}");
            StartCoroutine(WaitAndPlaySound(selectedSound.clip));
        }
        else
        {
            if (soundEffectSource.clip == selectedSound.clip)
            {
                soundEffectSource.Stop();
            }
        }
    }


    private IEnumerator WaitAndPlaySound(AudioClip clip)
    {
        while (soundEffectSource.isPlaying)
        {
            playing=true;
            yield return null;
        }
        playing=false;
        soundEffectSource.clip = clip;
        soundEffectSource.Play();
    }
}