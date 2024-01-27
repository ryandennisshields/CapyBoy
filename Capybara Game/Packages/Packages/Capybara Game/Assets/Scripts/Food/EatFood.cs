using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatFood : MonoBehaviour
{
    // The player's meter
    public Slider meter;

    private void FixedUpdate()
    {
        // Decrease the meter over time
        meter.value -= 0.05f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the other object that the player is colliding with has the tag "Food", add to the meter and destroy the food
        if (collision.gameObject.tag == "Food") 
        {
            meter.value += 10;
            Destroy(collision.gameObject);
        }
    }
}
