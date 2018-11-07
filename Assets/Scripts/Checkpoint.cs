using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    bool entered = false;

    void OnTriggerEnter2D()
    {
        entered = true;
    }
	
	// Update is called once per frame
	void Update () {
		if (entered)
        {
            Time.timeScale = 0f;
        }
	}
}
