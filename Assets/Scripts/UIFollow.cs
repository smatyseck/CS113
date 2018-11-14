using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour {

    [SerializeField]
    private Camera cameraScreen;

    [SerializeField]
    private Vector2 worldPositionOffset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        adjustTransformToObject();
	}

    private void adjustTransformToObject() {
        Vector2 toScreenSpace = cameraScreen.WorldToScreenPoint((Vector2) cameraScreen.transform.position + worldPositionOffset);
        this.transform.position = toScreenSpace;
    }
}
