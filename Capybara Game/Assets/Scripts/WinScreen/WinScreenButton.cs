using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenButton : MonoBehaviour
{
    // Specify all available button types.
    public enum ButtonTypes { ReturnToMenu };

    public ButtonTypes ButtonType;

    private GameManager gameManager;

    private Canvas parentCanvas;
    private bool previousParentCanvasVisibleState;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        parentCanvas = this.gameObject.GetComponentInParent<Canvas>();

        previousParentCanvasVisibleState = parentCanvas.enabled;
    }

    private void Update()
    {
        if (previousParentCanvasVisibleState != parentCanvas.enabled)
        {
            previousParentCanvasVisibleState = parentCanvas.enabled;

            if (parentCanvas.enabled)
            {
                this.gameObject.SetActive(false);
                #pragma warning disable CS4014
                    ShowButton();
                #pragma warning restore CS4014
            }
        }
    }

    private async Task ShowButton()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        this.gameObject.SetActive(true);
    }

    public void OnMouseClick()
    {

        // Validate which type of button we are using and action appropriately.
        switch (ButtonType)
        {

            case ButtonTypes.ReturnToMenu:
                // Hide win screen & return to menu.
                this.parentCanvas.enabled = false;
                Time.timeScale = 1;
                gameManager.ReturnToMainMenu();
                break;

            default:
                Debug.LogWarning("Invalid button called (can't happen scenario, make sure nothing is broken)");
                break;
        }
    }
}
