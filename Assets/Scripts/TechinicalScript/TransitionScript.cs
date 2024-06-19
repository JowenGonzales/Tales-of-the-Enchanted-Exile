using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] public GameObject startingSceneTransition;
    [SerializeField] public GameObject endingSceneTransition;

    private void Start()
    {
        if (startingSceneTransition != null)
        {
            startingSceneTransition.SetActive(true);
            StartCoroutine(DisableStartingTransition());
        }
    }

    private IEnumerator DisableStartingTransition()
    {
        yield return new WaitForSeconds(5);
        startingSceneTransition.SetActive(false);
    }

    public void StartTransition(string sceneName)
    {
        if (endingSceneTransition != null)
        {
            endingSceneTransition.SetActive(true);
            StartCoroutine(TransitionToScene(sceneName));
        }
    }

    private IEnumerator TransitionToScene(string sceneName)
    {
        yield return new WaitForSeconds(1); // Adjust the delay for transition
        SceneManager.LoadScene(sceneName);
    }
}
