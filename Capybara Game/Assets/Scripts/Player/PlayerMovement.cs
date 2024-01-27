using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public SpriteRenderer spriteRenderer;

    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 move;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate()
    {
        // Get player movement from Unity Input class.
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        // Set the animator's walking bool to "true" if the capybara is moving, false if not
        if (rb.velocity.x != 0 || rb.velocity.y != 0) 
        {
            animator.SetBool("Walking", true);
            // Flip the sprite depending on the movement direction
            if (rb.velocity.x > 0)
                spriteRenderer.flipX = true;
            else if (rb.velocity.x < 0)
                spriteRenderer.flipX = false;
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        // Normalise the updated Vector2 (magnitude of 1).
        move.Normalize();

        // Set RigidBody velocity.
        rb.velocity = (move * moveSpeed);
    }
}
