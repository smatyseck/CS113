using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {

	public int x_mag = 10;		//x modifier of velocity
	public int y_mag = 0;		//y modifier of velocity
    public bool isOn = true;
    public GameObject jumpText;

    private void Start()
    {
        jumpText.SetActive(false);
    }

    public void TurnOn()
    {
        isOn = true;
    }

    public void ToggleTextOff()
    {
        if (isOn)
        {
            jumpText.SetActive(false);
        }
    }

    public void ToggleTextOn()
    {
        if (isOn)
        {
            jumpText.SetActive(true);
        }
    }

    public int getX ()
	{
		return x_mag;
	}
	public int getY ()
	{
		return y_mag;
	}
}
