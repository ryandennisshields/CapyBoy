using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatFood : MonoBehaviour
{
    // The player's meter
    public GameObject[] filledIndicators;
    public int indicatorToFill = 0;

    [SerializeField] private AudioSource crunch;

    private GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the other object that the player is colliding with has the tag "Food", add to the meter and destroy the food
        if (collision.gameObject.tag == "Food") 
        {
            crunch.Play();

            if (indicatorToFill <= 4)
            {
                filledIndicators[indicatorToFill].SetActive(true);
                indicatorToFill++;
            }

            if (indicatorToFill >= 5)
            {
                gameManager.WinScreenCanvasVisible = true;
            }

            Destroy(collision.gameObject);
        }
    }

}
