using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Audio players components.
    public AudioSource EffectsSource;
    public AudioSource MusicSource;

    public AudioSource buttonSound;

    // Random pitch adjustment range.
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    // Singleton instance.
    public static SoundManager Instance;
    

    // Initialize the singleton instance.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // This keeps it alive during LoadScene
        }
        else
        {
            Destroy(gameObject); // Prevents "Double Music" when returning to menu
        }
    }

    // Play a single clip through the sound effects source.
    public void Play(AudioClip clip)
    {
        EffectsSource.clip = clip;
        EffectsSource.Play();

        AudioClip buttonSound = GetComponent<AudioClip>();
    }

    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

        EffectsSource.pitch = randomPitch;
        EffectsSource.clip = clips[randomIndex];
        EffectsSource.Play();
    }
    
    public void TurnItOff()
    {
        
    }

    private void OnEnable()
    {
        GameEvents.OnMoonResist += PlayButtonSound;
        GameEvents.OnQTEBegin += ReducePitch;
        GameEvents.OnPlayerImpact += resetPitch;
        GameEvents.OnVictoryAchieved += resetPitch;
    }

    private void OnDisable()
    {
        GameEvents.OnMoonResist -= PlayButtonSound;
        GameEvents.OnQTEBegin -= ReducePitch;
        GameEvents.OnPlayerImpact -= resetPitch;
        GameEvents.OnVictoryAchieved -= resetPitch;
    }

    public void ReducePitch()
    {
        // reduce the music pitch when we slow down time.
        // reduce time.
        MusicSource.pitch = 0.5f;
        Time.timeScale = 0.6f;
    }

    public void resetPitch()
    {
        // reset the music pitch when the QTE is over.
        MusicSource.pitch = 1.56f;
        // reset the time, also.
        Time.timeScale = 0.6f;
    }    

    // listen to E event.
    public void PlayButtonSound()
    {
        
        if (buttonSound != null)
        {
            buttonSound.Play();
        }
        else
        {
            Debug.LogWarning("SoundManager: You forgot to assign the AudioSource in the Inspector!");
        }
    }
}
