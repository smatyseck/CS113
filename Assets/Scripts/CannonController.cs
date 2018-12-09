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
        if (!isOn)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.3f, 0.3f, 0.3f);
        }
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
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
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
