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

    private GameObject gameManagerEntity;
    private GameManager gameManager;

    void Start()
    {
        gameManagerEntity = GameObject.Find("GameManager");
        gameManager = gameManagerEntity.GetComponent<GameManager>();
    }

    public void OnMouseClick()
    {

        // Validate which type of button we are using and action appropriately.
        switch (ButtonType)
        {
            case ButtonTypes.Start:
                gameManager.RequestLevelChange("Main Level");
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
