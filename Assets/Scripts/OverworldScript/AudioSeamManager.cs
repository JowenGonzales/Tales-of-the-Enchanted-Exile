using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioSeamManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;

    [Header("Audio Clip")]
    public AudioClip backgroundMusic;

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

    // Ensures there is only one instance of the music object and persists it across scenes
    private void Awake()
    {
        ManageSingleton();
    }

    // Subscribes to the sceneLoaded event when the object is enabled
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unsubscribes from the sceneLoaded event when the object is disabled
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Manages the singleton pattern for the GameMusic object
    private void ManageSingleton()
    {
        GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("OverworldMusic");
        if (musicObjs.Length > 1)
        {
            // Destroy the current object if there is already an instance
            Destroy(gameObject);
        }
        else
        {
            // Prevent the music object from being destroyed on scene load
            DontDestroyOnLoad(gameObject);
        }
    }

    // Stops the music when entering the MenuScene
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MenuScene")
        {
            // Destroy the music object when the MenuScene is loaded
            Destroy(gameObject);
        }
    }

    // Set the volume using the audio mixer
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}
