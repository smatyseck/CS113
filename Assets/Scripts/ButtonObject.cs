using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour {

    public GameObject objectToActivate;
    public GameObject pressedButton;
    public GameObject objectToDeactivate;

    private void Start()
    {
        pressedButton.SetActive(false);
    }

    public void Press()
    {
        objectToActivate.GetComponent<CannonController>().TurnOn();
        objectToDeactivate.SetActive(false);
        pressedButton.SetActive(true);
        gameObject.SetActive(false);
    }
}
