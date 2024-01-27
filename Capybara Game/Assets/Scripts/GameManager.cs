using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Boolean for handling game pause state.
    // Game should always start unpaused or unexpected behaviour may occur.
    private bool _GamePaused = false;
    public bool PauseState {
        // Ensure private variable can be passed to public.
        get { return _GamePaused; }
        // Ensure public variable can be passed to private.
        // Also, we want to do something when this is changed (check state, pause/unpause etc.)
        set
        {
            _GamePaused = _ValidatePauseChange(_GamePaused, value);
            if (_GamePaused)
            {
                _PauseGame();
            } else
            {
                _UnpauseGame();
            }
        }
    }

    private bool _PauseWimpUIVisible = false;
    public bool PauseWimpUIVisible
    {
        // Ensure private variable can be passed to public.
        get { return _PauseWimpUIVisible; }
        // Ensure public variable can be passed to private.
        // Also, we want to do something when this is changed (check state, pause/unpause etc.)
        set
        {
            _PauseWimpUIVisible = value;
            this.PauseWimpUICanvas.gameObject.SetActive(value);
        }
    }

    private bool _WinScreenCanvasVisible = false;
    public bool WinScreenCanvasVisible
    {
        // Ensure private variable can be passed to public.
        get { return _WinScreenCanvasVisible; }
        // Ensure public variable can be passed to private.
        // Also, we want to do something when this is changed (check state, pause/unpause etc.)
        set
        {
            _WinScreenCanvasVisible = value;
            this.WinScreenCanvas.gameObject.SetActive(value);

            if (value)
            {
                this.WinScreenCanvas.GetComponentInChildren<AudioSource>().Play();
            }
        }
    }

    // Array of Scenes to inhibit pause on.
    // Pausing here could cause things to break.
    private readonly string[] InhibitPauseScenes =
    {
        "Main Menu"
    };

    // Storage container for reference to Pause Canvas
    private Canvas PauseCanvas;
    private CanvasRenderer PauseWimpUICanvas;

    // Storage container for reference to WinScreen Canvas.
    private Canvas WinScreenCanvas;

    // Instantisation function.
    // Just makes sure this class remains in memory.
    private void Awake()
    {
        // Set up scene load detect event.
        SceneManager.sceneLoaded += this.OnNewLevelLoaded;

        DontDestroyOnLoad(this.gameObject);

        // Get pause canvas.
        PauseCanvas = this.gameObject.GetComponentsInChildren<Canvas>().ToList().Find(x => x.name.Contains("PauseCanvas"));
        PauseWimpUICanvas = this.PauseCanvas.gameObject.GetComponentsInChildren<CanvasRenderer>().ToList().Find(x => x.name.Contains("MenuReturnPanel"));
        PauseWimpUICanvas.gameObject.SetActive(false);

        WinScreenCanvas = this.gameObject.GetComponentsInChildren<Canvas>().ToList().Find(x => x.name.Contains("WinScreenCanvas"));
        Debug.Log(WinScreenCanvas);
        WinScreenCanvas.gameObject.SetActive(false);
    }

    // Level change request function.
    // Can implement logic here to check if a scene change is practical and
    // block if necessary.
    public void RequestLevelChange(string sceneName)
    {
        // Before change make sure that the scene we are transitioning to
        // does not inhibit pause. If it does, unpause the game.
        if (this.InhibitPauseScenes.Contains(sceneName))
        {
            // Pause is inhibited for the scene we are loading.
            // Unpause the game by force prior to scene change.
            this.PauseState = false;
        }

        SceneManager.LoadScene(sceneName);
    }

    // Simple return to menu stub function.
    public void ReturnToMainMenu()
    {
        this.RequestLevelChange("Main Menu");
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

    // Private function to validate the pause change on set.
    private bool _ValidatePauseChange(bool oldState, bool newState)
    {
        // Don't even bother going further if the old and new state are identical.
        if (oldState == newState) { return newState; }

        // Check scene. We do not want to pause on a scene where pause is inhibited.
        if (this.InhibitPauseScenes.Contains(SceneManager.GetActiveScene().name) && newState)
        {
            // Force set to unpaused if old state was paused (how did we pause?)
            if (oldState == true)
            {
                this._GamePaused = false;
            }

            // Forcefully return false as path has ended.
            return false;
        }

        // As all validations have passed at this point, allow
        // the new state to be passed to the setter.
       
        return newState;
    }

    // Private function to pause game
    private void _PauseGame()
    {
        // For now simply just set TimeScale to 0
        //Debug.Log("Game has been paused.");
        Time.timeScale = 0;

        // Display pause UI.
        PauseCanvas.enabled = true;
    }

    // Private function to unpause game
    private void _UnpauseGame()
    {
        // For now simply just set TimeScale to 1
        //Debug.Log("Game has been unpaused.");
        Time.timeScale = 1;

        // Hide pause UI.
        PauseCanvas.enabled = false;
    }

    // Update loop for pausing.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Invert current pause state.
            this.PauseState = !this.PauseState;
        }
    }

    // Event for scene loaded.
    private void OnNewLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play level music if it exists.
        if (GameObject.Find("LevelMusic"))
        {
            // Play level music.
            GameObject.Find("LevelMusic").GetComponent<AudioSource>().Play();
        }
    }
}
