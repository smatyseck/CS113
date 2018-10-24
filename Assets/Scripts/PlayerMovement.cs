using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    public GameObject otherPlayer;
    public float jumpForce;
    // What is the maximum speed we want Bob to walk at
    public float maxSpeed = 5f;

    // Start facing right (like the sprite-sheet)
    private bool facingLeft = false;

    // This will be a reference to the RigidBody2D 
    // component in the Player object
    private Rigidbody2D rb;

    // This is a reference to the Animator component
    private Animator anim;
    private bool grounded = false;

    // We initialize our two references in the Start method
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        grounded = true;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        grounded = true;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        grounded = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Reset"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetButtonDown("Swap"))
        {
            swap();
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

        if (grounded)
        {
            // Get the extent to which the player is currently pressing left or right
            float h = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(h * maxSpeed, rb.velocity.y);
            // Check which way the player is facing 
            // and call reverseImage if neccessary
            if (h < 0 && !facingLeft)
                reverseImage();
            else if (h > 0 && facingLeft)
                reverseImage();
        }

        anim.SetBool("grounded", grounded);
        anim.SetFloat("speedY", rb.velocity.y);
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        Debug.Log(grounded);

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
 