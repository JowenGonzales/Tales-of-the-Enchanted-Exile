using UnityEngine;
using UnityEngine.SceneManagement;

public class SeamlessMusic : MonoBehaviour
{
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
        GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("GameMusic");
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

    // Stops the music when entering the BattleScene, OverworldScene, or Cutscene
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BattleScene" || scene.name == "OverworldScene" || scene.name == "Cutscene")
        {
            // Destroy the music object when the BattleScene is loaded
            Destroy(gameObject);
        }
    }
}
