using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Manages all of the game audio and music of the game.
/// </summary>
public class MusicManager : MonoBehaviour {

    // Singleton Reference
    public static MusicManager musicManager;

    // Public Variable/References
    [Header("Audio Mixers")]
    public AudioMixer audioMixer;

    [Header("Audio Group")]
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup effectMixer;

    // Audio Sources.
    private AudioSource _musicSource;
    private AudioSource _eventSource;
    private AudioSource _feedbackSource;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Sets this object as the singleton to rule them all.
        if (musicManager == null)
            musicManager = this;

        // Get's the different audio-sources from both this object and child objects.
        _musicSource = GetComponent<AudioSource>();
        _eventSource = transform.GetChild(0).GetComponent<AudioSource>();
        _feedbackSource = transform.GetChild(1).GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a music track on the main source on request.
    /// </summary>
    public void PlayMusicTrack(AudioClip audioclip)
    {
        _musicSource.volume = 0.45f;
        _musicSource.clip = audioclip;
        _musicSource.Play();
    }

    /// <summary>
    /// Fades out the currently playing music track.
    /// </summary>
    public void FadeOutMusic()
    {
        StartCoroutine(FadeOutMusicEnumerator());
    }

    /// <summary>
    /// Fades out the currently playing music track over a period of time.
    /// </summary>
    IEnumerator FadeOutMusicEnumerator()
    {
        // Does a loop that will be used to calculate the volume.
        float currentTime = 0f;
        do
        {
            // Gets the amount of progress from 0 - 1 and sets it to the UI.
            float currentVolume = currentTime / 0.5f;
            _musicSource.volume = Mathf.Lerp(0.45f, 0, currentVolume);

            // Adds the time to the currentTime and returns the IEnnumerator
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime <= 0.5f + Time.deltaTime);
    }

    /// <summary>
    /// Plays a given event audio.
    /// </summary>
    public void PlayEventAudio(AudioClip audioClip)
    {
        _eventSource.PlayOneShot(audioClip);
    }

    /// <summary>
    /// Plays a given voice over.
    /// </summary>
    public void PlayFeedbackAudio(AudioClip audioClip)
    {
        _feedbackSource.PlayOneShot(audioClip);
    }
}
