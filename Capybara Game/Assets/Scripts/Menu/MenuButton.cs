using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    // Specify all available button types.
    public enum ButtonTypes { Start, Quit };

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
            case ButtonTypes.Start:
                gameManager.RequestLevelChange("Joshua Playtest");
                break;

            case ButtonTypes.Quit:
                gameManager.QuitGame();
                break;

            default:
                Debug.LogWarning("Invalid button called (can't happen scenario, make sure nothing is broken)");
                break;
        }
    }
}
