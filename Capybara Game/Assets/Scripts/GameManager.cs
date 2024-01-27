using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Level change request function.
    // Can implement logic here to check if a scene change is practical and
    // block if necessary.
    public void RequestLevelChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Quit game function.
    // Upon calling, the game will be put into a state that it is ready
    // to be shutdown. Then, it will be shutdown.
    public void QuitGame()
    {
        // Application.Quit cannot be called in the Editor.
        // So we provide debug logging feedback to show that the application WOULD have quit if not in Editor.
        if (Application.isEditor)
        {
            Debug.LogWarning("Attempted to quit game, but we are in the editor! (The game would have quit if not running in Editor)");
        } else
        {
            Application.Quit();
        }
    }

}
