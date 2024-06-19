using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{
    public float waitTime = 0f; // Time to wait before auto-loading the next scene
    public KeyCode skipKey = KeyCode.Space; // Key to skip the intro

    private bool introSkipped = false; // Flag to check if the intro was skipped

    void Start()
    {
        StartCoroutine(WaitForIntro());
    }

    void Update()
    {
        // Check for user input to skip the intro
        if (Input.GetKeyDown(skipKey))
        {
            introSkipped = true;
            SceneManager.LoadScene(3);
        }
    }

    IEnumerator WaitForIntro()
    {
        // Wait for the specified time or until the intro is skipped
        float elapsedTime = 0f;
        while (elapsedTime < waitTime && !introSkipped)
        {
            yield return null; // Wait for the next frame
            elapsedTime += Time.deltaTime;
        }

        // If the intro was not skipped, load the next scene
        if (!introSkipped)
        {
            SceneManager.LoadScene(3);
        }
    }
}
