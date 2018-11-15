﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerMovement : MonoBehaviour
{
    public int playerNum = 1; // 1 or 2 
    public GameObject otherPlayer; 
    public float jumpForce = 350;
    public GameObject readytext;
    public float maxSpeed = 5f;
    public float sprintMod = 1.5f;
    public float accelRate = 15; // inverse of rate that players accel at (15 means 15 updates until max speed)
    private bool grounded = false;
    private bool ready = false;

    // Start facing right (like the sprite-sheet)
    private bool facingLeft = false;

    // This will be a reference to the RigidBody2D 
    // component in the Player object
    private Rigidbody2D rb;

    // This is a reference to the Animator component
    private Animator anim;
    

	//Checkpoint Tracking
	public GameObject checkpoint;

    public bool IsReady()
    {
        // Check if ready to swap
        return ready;
    }

    public void SetReady(bool r)
    {
        // Set swap ready status
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
            if (col.gameObject != checkpoint) // If the checkpoint you just walked over is a new checkpoint, deactivate your previous checkpoint
            {
                checkpoint.GetComponent<CheckpointController>().Deactivate();
            }
			checkpoint = col.gameObject;
		}
		if (col.tag == "Zapper") // If you walk into a Zapper then send the player back to their checkpoint and set all velocities to 0
		{
			this.transform.position = checkpoint.transform.position;
            rb.velocity = new Vector2(0, 0);
		}
        if (col.tag == "Ground")
        {
            grounded = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Ground")
        { 
            grounded = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            grounded = false;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0) // Do not want users to be able to act when in a menu
        {
            return;
        }
        if (Input.GetButtonDown("Reset" + playerNum)) // Reload current level
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetButtonDown("Swap" + playerNum))
        {
            if (otherPlayer.GetComponent<PlayerMovement>().IsReady()) // If other player is ready, set all ready flags to false and swap
            {
                ready = false;
                otherPlayer.GetComponent<PlayerMovement>().SetReady(false);
                swap();
                readytext.SetActive(false);
            }
            else // If other player is not ready then set own ready status to true and advertise that you are ready
            {
                ready = true;
                Text text = readytext.GetComponent<Text>();
                text.text = "Player" + playerNum + "is Ready to Swap\n3";
                readytext.SetActive(true);
            }
        }

        if (Input.GetButtonDown("LastCheckpoint" + playerNum)) // Sends player to their checkpoint
        {
            rb.position = checkpoint.transform.position;
        }

        if (Input.GetButtonDown("Jump" + playerNum) && grounded) // Players can only jump when grounded
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

        if (grounded) // Players can only move when grounded (no air movement)
        {
            float h = Input.GetAxis("Horizontal" + playerNum); // Get the extent to which the player is currently pressing left or right
            if (Math.Abs(h) < 0.1f) // Some joysticks have an odd sensitivity and move when not pressed, this prevents that
            {
                h = 0;
            }
            float newspeed = 0;
            if ((rb.velocity.x > 0 && h < 0) || (rb.velocity.x < 0 && h > 0)) // Make players instantly move in the opposite direction
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                float limitSpeed = maxSpeed;
                if (Input.GetButtonDown("Sprint" + playerNum)) // If players are sprinting, allow for speed over max
                {
                    limitSpeed = maxSpeed * sprintMod;
                }
                newspeed = rb.velocity.x + (h * limitSpeed) / accelRate;
                if (newspeed > limitSpeed) // Restrict right moment to below the limit
                {
                    newspeed = limitSpeed;
                }
                else if (newspeed < -1 * limitSpeed) // Restrict left movement to below limit
                {
                    newspeed = -1 * limitSpeed;
                }
                if (h == 0) // Make players instantly stop when the let go of the movement button
                {
                    newspeed = 0f;
                }
                rb.velocity = new Vector2(newspeed, rb.velocity.y);
            }
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
        Vector2 pos2 = otherPlayer.GetComponent<Rigidbody2D>().position;

        rb.position = pos2;
        otherPlayer.GetComponent<Rigidbody2D>().position = pos1;
        //Swap Checkpoints as well
        GameObject c = otherPlayer.GetComponent<PlayerMovement>().checkpoint;
        otherPlayer.GetComponent<PlayerMovement>().checkpoint = checkpoint;
        checkpoint = c;
    }

}
 