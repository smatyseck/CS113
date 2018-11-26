using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapOff : MonoBehaviour {

    public float interval = 3f;
    private float timeremaining;
    public GameObject player1;
    public GameObject player2;

    // Use this for initialization
    void Start() {
        timeremaining = interval;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (!gameObject.activeSelf) {
            timeremaining = interval;
        }
        if (gameObject.activeSelf && timeremaining <= 0) {
            player1.GetComponent<PlayerMovement>().SetReady(false);
            player2.GetComponent<PlayerMovement>().SetReady(false);
            gameObject.SetActive(false);
            timeremaining = interval;
        }

        if (gameObject.activeSelf) {
            Text text = gameObject.GetComponent<Text>();
            timeremaining -= Time.deltaTime;
            if (player1.GetComponent<PlayerMovement>().IsReady()) {
                text.text = "Player 1 is Ready to Swap\n" + Mathf.Ceil(timeremaining).ToString();
            } else if (player2.GetComponent<PlayerMovement>().IsReady()) {
                text.text = "Player 2 is Ready to Swap\n" + Mathf.Ceil(timeremaining).ToString();
            }
        }
        
    }
    
}
