using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public GameObject player;
    public GameObject otherPlayer;
    // What is the maximum speed we want Bob to walk at
    private float maxSpeed = 5f;

    // Start facing right (like the sprite-sheet)
    private bool facingLeft = false;

    // This will be a reference to the RigidBody2D 
    // component in the Player object
    private Rigidbody2D rb;
    private Rigidbody2D rb2;

    // This is a reference to the Animator component
    private Animator anim;
    private Animator anim2;
    private bool jumping = false;
    public float jumpForce;


    // We initialize our two references in the Start method
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        anim = player.GetComponent<Animator>();

    }

    // We use FixedUpdate to do all the animation work
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            swap();
        }
        if (Input.GetButtonDown("Jump") && !jumping)
        {
            rb.AddForce(Vector2.up * jumpForce);
            jumping = true;
        }
        // Get the extent to which the player is currently pressing left or right
        float h = Input.GetAxis("Horizontal");
        if (rb.velocity.y > 0)
        {
            anim.SetBool("jump", true);
        }
        if (rb.velocity.y < 0)
        {
            anim.SetBool("falling", true);
        }
        else if (Mathf.Abs(rb.velocity.y) < 0.1)
        {
            jumping = false;
            anim.SetBool("jump", false);
            anim.SetBool("falling", false);
            rb.velocity = new Vector2(h * maxSpeed, rb.velocity.y);
            anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
            
        }

        // Check which way the player is facing 
        // and call reverseImage if neccessary
        if (h < 0 && !facingLeft)
            reverseImage();
        else if (h > 0 && facingLeft)
            reverseImage();

    }

    void reverseImage()
    {
        // Switch the value of the Boolean
        facingLeft = !facingLeft;

        // Get and store the local scale of the RigidBody2D
        Vector2 theScale = rb.transform.localScale;

        // Flip it around the other way
        theScale.x *= -1;
        rb.transform.localScale = theScale;
    }

    void swap()
    {
        Vector2 pos1 = rb.position;
        Vector2 pos2 = otherPlayer.GetComponent<Rigidbody2D>().position;

        rb.position = pos2;
        otherPlayer.GetComponent<Rigidbody2D>().position = pos1;
    }
}
 