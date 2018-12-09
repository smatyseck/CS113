using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameFade : MonoBehaviour {
    public GameObject firstText;
    public GameObject secondText;
    private int frame = 1;
    private float frametime = 180f;

    // Use this for initialization
    void Start () {
        firstText.GetComponent<Text>().color = new Color(1, 1, 1, 0);
        secondText.GetComponent<Text>().color = new Color(1, 1, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (frame <= frametime)
        {
            firstText.GetComponent<Text>().color = new Color(1, 1, 1, frame/frametime);
        }
        else if (frame <= frametime*2)
        {
            firstText.GetComponent<Text>().color = new Color(1, 1, 1, 1-((frame-frametime)/ frametime*2));
        }
        else if (frame <= frametime*3)
        {
            secondText.GetComponent<Text>().color = new Color(1, 1, 1, (frame- frametime*2) /frametime*3);
        }
        else if (frame <= frametime*4)
        {
            secondText.GetComponent<Text>().color = new Color(1, 1, 1, 1-((frame- frametime*3))/frametime*4);
        }
        if (frame == frametime * 5)
        {
            SceneManager.LoadScene("MainMenu");
        }
        frame++;
    }
}
