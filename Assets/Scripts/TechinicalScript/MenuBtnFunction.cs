using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBtnFunction : MonoBehaviour
{
    //Functions for going to different screens
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
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
}
