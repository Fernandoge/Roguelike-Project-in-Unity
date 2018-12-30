using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSpriteFromMousePos : MonoBehaviour {

	public Sprite spLeft, spRight, spUp, spDown;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F)) 
			Debug.Log(Input.mousePosition); 

		if(Input.mousePosition.y > 308)
			GetComponent<SpriteRenderer>().sprite = spUp;
		else
			GetComponent<SpriteRenderer>().sprite = spDown;
	}
}
