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
        // Initialise 2D rigidbody on start.
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        // Get movement axis from Input class, store in Vector2.
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        // Make vector have magnitude of 1.
        move.Normalize();

        // Set player velocity.
        rb.velocity = ( move * ( moveSpeed ) );

    }
}
