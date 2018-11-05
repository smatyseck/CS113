using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player1Movement : MonoBehaviour
{

    public GameObject player2;
    public float jumpForce = 350;
    public GameObject readytext;
    public GameObject pausemenu;

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
    private bool ready = false;
    
	//Checkpoint Tracking
	public GameObject checkpoint;

	public bool IsReady()
	{
		return ready;
	}

	public void SetReady(bool r)
	{
		ready = r;
	}

	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody2D>();
		anim = gameObject.GetComponent<Animator>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Checkpoint") 
		{
			checkpoint = col.gameObject;
		}
		if (col.tag == "Zapper") 
		{
			print ("zapped");
			this.transform.position = checkpoint.transform.position;
		}
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
        if (Input.GetButtonDown("Cancel"))
        {
            Time.timeScale = 0f;
            pausemenu.SetActive(true);
        }

        if (Input.GetButtonDown("Reset"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetButtonDown("Swap"))
        {
            if (player2.GetComponent<Player2Movement>().IsReady())
            {
                swap();
                readytext.SetActive(false);
            }
            else {
                ready = true;
                Text text = readytext.GetComponent<Text>();
                text.text = "Player 1 is Ready to Swap\n3";
                readytext.SetActive(true);
            }   
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

        if (Input.GetButton("Sprint"))
        {
            maxSpeed = 10f;
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
        Vector2 pos2 = player2.GetComponent<Rigidbody2D>().position;

        rb.position = pos2;
        player2.GetComponent<Rigidbody2D>().position = pos1;
    }

}
 