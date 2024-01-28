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
            if (rb.velocity.y > 0 && rb.velocity.x == 0)
                animator.SetBool("WalkingUp", true);
            else
                animator.SetBool("WalkingUp", false);
            if (rb.velocity.y < 0 && rb.velocity.x == 0)
                animator.SetBool("WalkingDown", true);
            else
                animator.SetBool("WalkingDown", false);
            if (rb.velocity.x > 0)
            {
                animator.SetBool("WalkingHorizontal", true);
                spriteRenderer.flipX = true;
            }
            else if (rb.velocity.x < 0)
            {
                animator.SetBool("WalkingHorizontal", true);
                spriteRenderer.flipX = false;
            }
            else
                animator.SetBool("WalkingHorizontal", false);
        }
        else
        {
            animator.SetBool("WalkingUp", false);
            animator.SetBool("WalkingDown", false);
            animator.SetBool("WalkingHorizontal", false);
        }

        // Normalise the updated Vector2 (magnitude of 1).
        move.Normalize();

        // Set RigidBody velocity.
        rb.velocity = (move * moveSpeed);
    }
}
