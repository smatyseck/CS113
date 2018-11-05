using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

	public Sprite inactivePlatform;
	public Sprite activePlatform;
	private SpriteRenderer mySpriteRenderer;
	public bool activated;

	// Use this for initialization
	void Start () {
		print("CheckpointController Active");
		mySpriteRenderer = this.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2d(Collider2D other)
	{
		print("collision detected");
		if (other.tag == "Player") {
			mySpriteRenderer.sprite = activePlatform;
			activated = true;
		}
	}
}
