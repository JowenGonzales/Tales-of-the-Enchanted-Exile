using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsFunction : MonoBehaviour
{
    // References for Audio Mixer, Slider, Toggle, and AudioManager
    public AudioMixer audioMixer;
    public Slider volSlider;
    public Toggle fullscreenToggle;
    public AudioManager audioManager;

    private void Start()
    {
        SetDefaultResolution();         // Set default resolution
        LoadSettings();                 // Load and apply saved settings
        AddListeners();                 // Add event listeners to UI elements
    }

    private void SetDefaultResolution()
    {
        // Set the default resolution to 1920 x 1080
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }

    private void LoadSettings()
    {
        // Load and apply saved volume settings
        float savedVolume = PlayerPrefs.GetFloat("volume", Mathf.Log10(0.0001f) * 20);
        volSlider.value = Mathf.Pow(10, savedVolume / 20); // Convert back from dB
        audioMixer.SetFloat("volume", savedVolume);

        // Load and apply saved fullscreen state
        bool isFullScreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    private void AddListeners()
    {
        // Add listeners for UI elements
        volSlider.onValueChanged.AddListener(SetVolume);
        fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void RemoveListeners()
    {
        // Remove listeners to prevent memory leaks
        volSlider.onValueChanged.RemoveListener(SetVolume);
        fullscreenToggle.onValueChanged.RemoveListener(SetFullScreen);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        // Set fullscreen mode and save the state
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("fullscreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetVolume(float volume)
    {
        // Set volume in dB and save the state
        float volumeInDb = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("volume", volumeInDb);
        PlayerPrefs.SetFloat("volume", volumeInDb);
        PlayerPrefs.Save();

        // Update the AudioManager volume if available
        if (audioManager != null)
        {
            audioManager.SetVolume(volumeInDb);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reload settings when a new scene is loaded
        LoadSettings();
    }

    private void OnDestroy()
    {
        // Remove listeners to prevent memory leaks
        RemoveListeners();
    }
}
