using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatFood : MonoBehaviour
{
    // The player's meter
    public GameObject[] filledIndicators;
    private int indicatorToFill = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the other object that the player is colliding with has the tag "Food", add to the meter and destroy the food
        if (collision.gameObject.tag == "Food") 
        {
            if (indicatorToFill <= 4)
            {
                filledIndicators[indicatorToFill].SetActive(true);
                indicatorToFill++;
            }
            Destroy(collision.gameObject);
        }
    }
}
