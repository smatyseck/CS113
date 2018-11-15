using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public GameObject canvas;

	// Use this for initialization
	void Start () {
        canvas.SetActive(false);
	}

    public void Activate()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
        canvas.SetActive(true);
    }

    public void Deactivate()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        canvas.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
