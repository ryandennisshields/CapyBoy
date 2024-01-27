using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;

    public Rigidbody2D rb;

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

        // Normalise the updated Vector2 (magnitude of 1).
        move.Normalize();

        // Set RigidBody velocity.
        rb.velocity = (move * moveSpeed);
    }
}
