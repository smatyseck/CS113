﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

	public Sprite inactivePlatform;
	public Sprite activePlatform;
	private SpriteRenderer mySpriteRenderer;
	public bool activated;

	// Use this for initialization
	void Start () {
		mySpriteRenderer = this.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") {
			mySpriteRenderer.sprite = activePlatform;
			activated = true;
		}
	}

    public void Deactivate()
    {
        mySpriteRenderer.sprite = inactivePlatform;
        activated = false;
    }


}
