using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButton : MonoBehaviour
{
    // Specify all available button types.
    public enum ButtonTypes { Unpause, ReturnToMenu, WimpMenuConfirmMenu, WimpMenuConfirmContinue };

    public ButtonTypes ButtonType;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void OnMouseClick()
    {

        // Validate which type of button we are using and action appropriately.
        switch (ButtonType)
        {
            case ButtonTypes.Unpause:
                gameManager.PauseState = false;
                break;

            // Make player confirm their quit.
            case ButtonTypes.ReturnToMenu:
                gameManager.PauseWimpUIVisible = true;
                break;

            case ButtonTypes.WimpMenuConfirmMenu:
                // Return to menu.
                gameManager.ReturnToMainMenu();
                break;

            case ButtonTypes.WimpMenuConfirmContinue:
                gameManager.PauseWimpUIVisible = false;
                break;

            default:
                Debug.LogWarning("Invalid button called (can't happen scenario, make sure nothing is broken)");
                break;
        }
    }
}
