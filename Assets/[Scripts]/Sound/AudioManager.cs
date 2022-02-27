using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Enum for the main 3 tracks
    public enum MusicTrack
    {
        BGM_StartScene,
        BGM_PlayScene,
        BGM_EndScene
    }

    #region Singleton and DontDestroyOnLoad
    // Singleton design pattern implemented
    private static AudioManager _instance = null;

    /// <summary>
    /// Get Instance of the singleton
    /// </summary>
    /// <returns></returns>
    public static AudioManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<AudioManager>();
            DontDestroyOnLoad(_instance);
        }

        return _instance;
    }
    #endregion

    // Audio source to play
    private AudioSource source;

    // List of all the music tracks
    [SerializeField]
    private List<AudioClip> musicTracks;

    [SerializeField]
    private float maxVolume = 0.5f;

    /// <summary>
    /// Using Awake instead of Start for obvious reasons..
    /// </summary>
    private void Awake()
    {
        // Get the source component
        source = GetComponent<AudioSource>();

        // Initiates the singleton _instance, puts it on DDOL, and starts the basic track
        if (_instance == null)
        {
            GetInstance().PlayTrack(MusicTrack.BGM_StartScene);
        }

        // remove the clones
        RemoveCloneAudioManager();
    }

    /// <summary>
    /// Destroy (all) the clones of Audio Manager in the scene
    /// </summary>
    private void RemoveCloneAudioManager()
    {
        AudioManager[] clones = FindObjectsOfType<AudioManager>();

        // If the audio manager is not this one, destroy it immediately
        foreach (var clone in clones)
        {
            if (clone != _instance)
            {
                Destroy(clone.gameObject);
            }
        }
    }

    /// <summary>
    /// Play Track using the enum's reference
    /// </summary>
    /// <param name="trackNumber"></param>
    private void PlayTrack(MusicTrack trackNumber)
    {
        source.clip = musicTracks[(int)trackNumber];
        source.Play(0);
    }

    /// <summary>
    /// Perform a scene transition music track play by using a coroutine
    /// </summary>
    /// <param name="track">Track ID</param>
    /// <param name="delay">Delay to fade out, delay to fade in</param>
    public void PlaySceneTrack(MusicTrack track, float delay, float volume)
    {
        StartCoroutine(SceneTranstion_FadeOutFadeIn(track, delay, volume));
    }

    /// <summary>
    /// Sets the current volume of the background tracks
    /// </summary>
    /// <param name="volume">Volume value 0 to 1</param>
    public void SetVolume(float volume)
    {
        if (volume > maxVolume)
        {
            source.volume = maxVolume;
        }
        else
        {
            source.volume = volume;
        }

    }

    /// <summary>
    /// Returns max volume
    /// </summary>
    /// <returns></returns>
    public float GetMaxVolume()
    {
        return maxVolume;
    }


    /// <summary>
    /// Play's SFX with separate 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    public void PlaySFX(AudioSource source, AudioClip clip, float volume, bool loop)
    {
        float tempVolume = 0.5f;

        if (volume > 0.5f)
        {
            tempVolume = 0.5f;
        }
        else
        {
            tempVolume = volume;
        }

        source.volume = tempVolume;
        source.playOnAwake = false;
        source.loop = loop;
        source.clip = clip;
        source.Play();
    }


    #region FadeOutFadeIn
    /// <summary>
    /// Coroutine to have a fade in effect
    /// and fade out effect
    /// </summary>
    /// <param name="track">track id</param>
    /// <param name="delay">delay</param>
    /// <returns></returns>
    private IEnumerator SceneTranstion_FadeOutFadeIn(MusicTrack track, float delay, float volume)
    {
        // Initial transition
        float timer = 0.0f;

        // Perform Fade-Out using the provided delay
        while (timer < delay)
        {
            timer += Time.deltaTime;

            // Smooth step function to lerp smoothly
            source.volume = Mathf.SmoothStep(volume, 0.0f, timer / delay);

            yield return new WaitForEndOfFrame();
        }

        // Source volume is not 0, but close to epsilon, make is 0
        source.volume = 0.0f;

        // Reset timer
        timer = 0.0f;

        // Start the new track
        PlayTrack(track);

        // Perform Fade-In using the same delay
        while (timer < delay)
        {
            timer += Time.deltaTime;

            // Smooth step again
            source.volume = Mathf.SmoothStep(0.0f, volume, timer / delay);

            yield return new WaitForEndOfFrame();
        }

        // Source volume is not maxed... set it to max
        source.volume = volume;
    }
    #endregion

}
