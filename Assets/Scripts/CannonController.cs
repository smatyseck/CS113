using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour {

	public int x_mag = 10;		//x modifier of velocity
	public int y_mag = 0;		//y modifier of velocity

	public int getX ()
	{
		return x_mag;
	}
	public int getY ()
	{
		return y_mag;
	}
}
