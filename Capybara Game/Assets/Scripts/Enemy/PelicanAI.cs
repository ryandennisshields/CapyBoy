using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class PelicanAI : MonoBehaviour
{
    private AIDestinationSetter aiDestination;
    private AIPath aiPath;
    private LevelController levelController;

    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    private float deathTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Set the ai destination to the player, grab the level controller and ai path, and set the death timer
        aiDestination = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        aiDestination.target = GameObject.Find("Player").transform;
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        deathTimer = 10;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If colliding with a game object with the food tag, destroy it
        if (collision.gameObject.tag == "Food")
        {
            Destroy(collision.gameObject);
        }
        // If colliding with the player, remove their held item and remove the pelican
        if (collision.gameObject.name == "Player")
        {
            // Function needs to be made to do this stuff
            //levelController.PlayerIsHoldingItem = false;
            //levelController.PlayerHoldingItemType = null;
            //levelController.FoodHoldingUIElement.GetComponent<Image>().gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        // Update the death timer and remove the pelican when it reaches 0
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
        {
            Destroy(this.gameObject);
        }

        // Flip the sprite towards the player
        if (aiDestination.target.transform.position.x > gameObject.transform.position.x)
            spriteRenderer.flipX = true;
        else if (aiDestination.target.transform.position.x < gameObject.transform.position.x)
            spriteRenderer.flipX = false;
    }
}
