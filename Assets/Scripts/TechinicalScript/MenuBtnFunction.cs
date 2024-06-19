using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtnFunction : MonoBehaviour
{ 
    public TransitionScript transition;

    //Functions for going to different screens
    public void PlayGame()
    {
        transition.endingSceneTransition.SetActive(true);
        StartCoroutine(StartGame());
    }
    //Function for going to settings
    public void GoSettings()
    {
        SceneManager.LoadSceneAsync(1);
    }
    //Function for quiting
    public void QuitGame()
    {
        Application.Quit();
    }
    //Function for going back to menu
    public void BackMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1);       
        SceneManager.LoadScene(2);
    }


}
