using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
                if (!this.WinScreenCanvasVisible)
                {
                    _UnpauseGame();
                }
            }
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
            this.WinScreenCanvas.enabled = value;

            // WinScreenCanvas can be null in dev mode.
            if (value && this.WinScreenCanvas != null)
            {
                // Stop level music if it exists.
                if (GameObject.Find("LevelMusic"))
                {
                    // Play level music.
                    GameObject.Find("LevelMusic").GetComponent<AudioSource>().Stop();
                }

                this.WinScreenCanvas.GetComponentInChildren<AudioSource>().Play();

                // Pause the game (bypass GameManager for this).
                Time.timeScale = 0;
            }
        }
    }

    private bool _FailScreenCanvasVisible = false;
    public bool FailScreenCanvasVisible
    {
        get { return _FailScreenCanvasVisible; }
        set
        {
            this._FailScreenCanvasVisible = value;
            // FailScreen can be null in dev mode.
            if (this.FailScreenCanvas != null)
            {
                this.FailScreenCanvas.enabled = value;

                if (value)
                {
                    // Stop level music if it exists.
                    if (GameObject.Find("LevelMusic"))
                    {
                        // Play level music.
                        GameObject.Find("LevelMusic").GetComponent<AudioSource>().Stop();
                    }

                    this.FailScreenCanvas.GetComponentInChildren<AudioSource>().Play();

                    // Pause the game (bypass GameManager for this).
                    Time.timeScale = 0;
                }
            } else
            {
                if (value)
                {
                    Debug.LogWarning("Would have shown fail screen here, but it doesn't exist in this context.");
                }
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

    // Storage container for reference to FailScreen Canvas.
    private Canvas FailScreenCanvas;

    // Instantisation function.
    // Just makes sure this class remains in memory.
    private void Awake()
    {
        // Set up scene load detect event.
        SceneManager.sceneLoaded += this.OnNewLevelLoaded;

        if (GameObject.FindGameObjectsWithTag("GameManager").Length > 1)
        {
            // GameManager already exists, having two will cause undefined
            // behaviour.
            Destroy(this.gameObject);
        } else
        {
            // We can instantiate.
            DontDestroyOnLoad(this.gameObject);
        }
        

        // ONLY INSTANTIATE THESE WHEN THEY EXIST!
        if (this.gameObject.GetComponentsInChildren<Canvas>().Length > 0)
        {
            // Get pause canvas.
            PauseCanvas = this.gameObject.GetComponentsInChildren<Canvas>().ToList().Find(x => x.name.Contains("PauseCanvas"));

            WinScreenCanvas = this.gameObject.GetComponentsInChildren<Canvas>().ToList().Find(x => x.name.Contains("WinScreenCanvas"));
            if (WinScreenCanvas != null)
            {
                WinScreenCanvas.enabled = false;
            }

            FailScreenCanvas = this.gameObject.GetComponentsInChildren<Canvas>().ToList().Find(x => x.name.Contains("FailCanvas"));
            if (FailScreenCanvas != null)
            {
                FailScreenCanvas.enabled = false;
            }
        } else
        {
            Debug.LogWarning("GameManager has been instantiated by the playtest stub. Pause menu and win screen will not be available.");
        }

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

    // Restart level button.
    // Just reload the level we are on.
    public void RestartLevel()
    {
        this.RequestLevelChange(SceneManager.GetActiveScene().name);
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

        // Don't allow pause screen to override win screen.
        if (this.WinScreenCanvasVisible)
        {
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
        Time.timeScale = 0;

        // Display pause UI if instantiated.
        if (PauseCanvas != null)
        {
            PauseCanvas.enabled = true;
        }
    }

    // Private function to unpause game
    private void _UnpauseGame()
    {
        // For now simply just set TimeScale to 1
        Time.timeScale = 1;

        // Hide pause UI if instantiated.
        if (PauseCanvas != null)
        {
            PauseCanvas.enabled = false;
        }
    }

    // Update loop for pausing.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseCanvas != null)
            {
                // Invert current pause state.
                this.PauseState = !this.PauseState;
            } else
            {
                // Check if in editor to determine which exit method to use.
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
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
