using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D body; //the player's rigidbody, allows forces to be applied to the player

    public float moveSpeed; //how fast the player moves

    Vector2 axis;

    void Start()
    {
        body = GetComponent<Rigidbody2D>(); //access the player's rigidbody and call it "body"
    }

   
    void Update()
    {
        axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * moveSpeed;
        body.AddForce(axis, ForceMode2D.Force);
    }

}
