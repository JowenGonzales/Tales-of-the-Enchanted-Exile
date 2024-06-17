using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Audio sources for music and sound effects
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    // Audio clips for various sounds
    [Header("Audio Clip")]
    public AudioClip backgroundMusic;
    public AudioClip attack;
    public AudioClip magicCast;
    public AudioClip run;
    public AudioClip takeDamage;
    public AudioClip death;
    public AudioClip victory;

    // Audio mixer for controlling volume
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    public void Start()
    {
        // Set and play the background music
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    // Play a sound effect
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    // Set the volume using the audio mixer
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}
