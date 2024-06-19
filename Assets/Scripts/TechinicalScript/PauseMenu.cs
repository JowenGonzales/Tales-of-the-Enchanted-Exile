using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUi;
    private AudioSeamManager audioSeamManager;
    private TransitionScript transitionScript;

    private void Start()
    {
        // Find the AudioSeamManager instance in the scene
        audioSeamManager = FindObjectOfType<AudioSeamManager>();

        // Find the TransitionScript instance in the scene
        transitionScript = FindObjectOfType<TransitionScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        if (audioSeamManager != null)
        {
            audioSeamManager.SetVolume(-20f); // Lower the volume when paused
        }
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        if (audioSeamManager != null)
        {
            audioSeamManager.SetVolume(0f); // Restore the volume when resumed
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Resume();
        transitionScript.endingSceneTransition.SetActive(true);
        PlayerPrefs.DeleteAll();
        StartCoroutine(TransitionToMenu());
    }

    public void QuitGame()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    private IEnumerator TransitionToMenu()
    {
        yield return new WaitForSeconds(2); // Adjust the delay for transition
        SceneManager.LoadScene(0);
    }
}
