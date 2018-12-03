using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour {

    public GameObject objectToActivate;
    public GameObject pressedButton;

    private void Start()
    {
        pressedButton.SetActive(false);
    }

    public void Press()
    {
        objectToActivate.GetComponent<CannonController>().TurnOn();
        pressedButton.SetActive(true);
        gameObject.SetActive(false);
    }
}
