using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Either 1 or 2 to signify which player it is")]
    public int playerNum = 1; // 1 or 2 
    public GameObject otherPlayer; 
    public float jumpForce = 350;
    public GameObject readytext;
    public float maxSpeed = 5f;
    public float sprintMod = 1.5f;
    public float accelRate = 15f; // inverse of rate that players accel at (15 means 15 updates until max speed)
    public float airAccelModifier = .25f; // how much the speed will change while in air in respect to ground
    private bool grounded = false;
    private bool ready = false;
    private bool inExit = false;
    public bool shot = false;
    private bool atCannon = false;
    private bool onButton = false;
    private GameObject button;

    private CannonController cannonScript = null;

    // Start facing right (like the sprite-sheet)
    private bool facingLeft = false;

    // This will be a reference to the RigidBody2D 
    // component in the Player object
    private Rigidbody2D rb;

    // This is a reference to the Animator component
    private Animator anim;
    

	//Checkpoint Tracking
	public GameObject checkpoint;

    // Camera Tracking
    private enum CameraTrackingEnum {
        ALWAYS_TRACK_PLAYER,
        FOLLOW_PLAYER_ON_LEVEL
    }
    [SerializeField]
    private CameraTrackingEnum CameraTrackType;
    private Camera cameraBot;
    private Camera cameraTop;
    
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
        Transform cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder").transform;
        cameraTop = cameraHolder.GetChild(0).GetComponent<Camera>();
        cameraBot = cameraHolder.GetChild(1).GetComponent<Camera>();
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
		else if (col.tag == "Zapper") // If you walk into a Zapper then send the player back to their checkpoint and set all velocities to 0
		{
			this.transform.position = checkpoint.transform.position;
            col.GetComponent<AudioSource>().Play();
            rb.velocity = new Vector2(0, 0);
            shot = false;
            rb.gravityScale = 0.8f;
		}
        else if (col.tag == "Cannon" && col.GetComponent<CannonController>().isOn)
        {
            col.GetComponent<CannonController>().ToggleTextOn();
            atCannon = true;
			cannonScript = col.gameObject.GetComponent<CannonController> ();
        }
        else if (col.tag == "Exit")
        {
            inExit = true;
        }
        else if (col.tag == "Ground")
        {
            grounded = true;
        } else if (col.tag == "Wall")
        {
            if (shot)
            {
                rb.gravityScale = 0.8f;
                shot = false;
            }
        } else if (col.tag == "Button")
        {
            onButton = true;
            button = col.gameObject;
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
        else if (col.tag == "Exit")
        {
            inExit = false;
        }
        else if (col.tag == "Cannon")
        {
            col.GetComponent<CannonController>().ToggleTextOff();
            atCannon = false;
        }
        else if (col.tag == "Button")
        {
            onButton = false;
            button = null;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0) // Do not want users to be able to act when in a menu
        {
            return;
        }
        if (playerNum == 1 && inExit && otherPlayer.GetComponent<PlayerMovement>().inExit)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (Input.GetButtonDown("Reset" + playerNum)) // Reload current level
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (onButton && rb.velocity.y == 0)
        {
            button.GetComponent<ButtonObject>().Press();
        }

        if (atCannon) //In or shot out of a cannon
        {
            if (shot)
            {
                grounded = false;
				rb.velocity = new Vector2(cannonScript.getX(), cannonScript.getY());
            } else if ( atCannon && Input.GetButtonDown("Jump" + playerNum))
            {
                shot = true;
                grounded = false;
				rb.velocity = new Vector2(cannonScript.getX(), cannonScript.getY());
                rb.position = new Vector2(rb.position.x, rb.position.y + .25f);
                rb.gravityScale = 0;
            } 
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
            grounded = false;
            gameObject.GetComponent<AudioSource>().Play();
        }

        if (!shot)
        {
            float h = Input.GetAxisRaw("Horizontal" + playerNum); // Get the extent to which the player is currently pressing left or right
            if (Math.Abs(Input.GetAxis("Horizontal" + playerNum)) < 0.1f) // Some joysticks have an odd sensitivity and move when not pressed, this prevents that
            {
                h = 0;
            }
            float newspeed = 0;
            // Make players instantly move in the opposite direction while grounded
            if (((rb.velocity.x > 0 && h < 0) || (rb.velocity.x < 0 && h > 0)) && grounded)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                float limitSpeed = maxSpeed;
                if (Input.GetButton("Sprint" + playerNum)) // If players are sprinting, allow for speed over max
                {
                    limitSpeed = maxSpeed * sprintMod;
                }
                float speedChange = (h * limitSpeed) / accelRate;
                speedChange *= grounded ? 1 : airAccelModifier;
                newspeed = rb.velocity.x + speedChange;
                if (newspeed > limitSpeed) // Restrict right moment to below the limit
                {
                    newspeed = limitSpeed;
                }
                else if (newspeed < -1 * limitSpeed) // Restrict left movement to below limit
                {
                    newspeed = -1 * limitSpeed;
                }
                if (h == 0 && grounded) // Make players instantly stop when the let go of the movement button
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

    void swap() {
        readytext.GetComponent<SwapOff>().resetTime();
        Vector2 pos1 = rb.position;
        Vector2 pos2 = otherPlayer.GetComponent<Rigidbody2D>().position;
        if (shot) {
            pos2 = new Vector2(pos2.x, pos2.y + .25f);
        }
        if (otherPlayer.GetComponent<PlayerMovement>().shot) {
            pos1 = new Vector2(rb.position.x, rb.position.y + .25f);
        }

        transform.position = pos2;
        otherPlayer.transform.position = pos1;
        // Simply look at y position and adjust which player's on top track and which is on bottom track
        if (CameraTrackType == CameraTrackingEnum.FOLLOW_PLAYER_ON_LEVEL) {
            if (pos1.y > pos2.y) {
                adjustCameraTrack(otherPlayer, gameObject);
            }
            else {
                adjustCameraTrack(gameObject, otherPlayer);
            }
        }

        SlowTime.SetSlow(2f);
        
        //Swap Checkpoints as well
        GameObject c = otherPlayer.GetComponent<PlayerMovement>().checkpoint;
        otherPlayer.GetComponent<PlayerMovement>().checkpoint = checkpoint;
        checkpoint = c;
    }

    void adjustCameraTrack(GameObject newTop, GameObject newBot) {
        Follow topFollow = cameraTop.GetComponent<Follow>();
        topFollow.objectToFollow = newTop;

        Follow botFollow = cameraBot.GetComponent<Follow>();
        botFollow.objectToFollow = newBot;

        StopAllCoroutines();
        // Pause for 2 frames
        StartCoroutine(pauseCameras(new Follow[] { topFollow, botFollow }, Time.fixedUnscaledDeltaTime * 2));
    }

    IEnumerator pauseCameras(Follow[] cameraFollows, float pauseTime = .05f) {
        foreach (Follow camFollow in cameraFollows) {
            camFollow.follow = false;
        }
        yield return new WaitForSeconds(pauseTime);
        foreach (Follow camFollow in cameraFollows) {
            camFollow.follow = true;
        }
    }

}
 