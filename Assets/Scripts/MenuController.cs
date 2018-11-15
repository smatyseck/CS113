using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
    public GameObject pauseMenu;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Cancel"))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.GetComponent<PauseMenu>().Deactivate();
            }
            else
            {
                pauseMenu.GetComponent<PauseMenu>().Activate();
            }
            
        }
	}
}
