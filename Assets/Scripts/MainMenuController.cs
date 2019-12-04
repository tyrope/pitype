using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // TODO Something something load a new game.
        SceneManager.LoadSceneAsync("Game Screen", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("We tried to quit the application, but if you see this then you're in the editor and can't use Application.Quit()!");
        Application.Quit();
    }
}
