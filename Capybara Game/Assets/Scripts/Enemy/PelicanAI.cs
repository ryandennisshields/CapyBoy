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
    private AudioSource levelMusic;

    public AudioSource pelicanMusic;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    private float deathTimer;

    // Start is called before the first frame update
    void Start()
    {
        // Set the ai destination to the player, grab the level controller, level music and ai path, and set the death timer
        aiDestination = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        aiDestination.target = GameObject.Find("Player").transform;
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        levelMusic = GameObject.Find("LevelMusic").GetComponent<AudioSource>();
        levelMusic.Stop();
        pelicanMusic.Play();
        deathTimer = 10;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If colliding with a game object with the food tag, destroy it
        if (collision.gameObject.tag == "Food")
        {
            Destroy(collision.gameObject);
        }
        // If colliding with the player, remove their held item, a point, and remove the pelican
        if (collision.gameObject.name == "Player")
        {
            levelController.PlayerTouchedByPelican();
            pelicanMusic.Stop();
            levelMusic.Play();
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
            pelicanMusic.Stop();
            levelMusic.Play();
        }

        // Flip the sprite towards the player
        if (aiDestination.target.transform.position.x > gameObject.transform.position.x)
            spriteRenderer.flipX = true;
        else if (aiDestination.target.transform.position.x < gameObject.transform.position.x)
            spriteRenderer.flipX = false;
    }
}
