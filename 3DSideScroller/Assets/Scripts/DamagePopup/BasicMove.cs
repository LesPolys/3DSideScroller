using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		DamagePopUpController.Initialize ();	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.UpArrow)){
			transform.position += new Vector3 (0, 0, 1);
		}

		if(Input.GetKey(KeyCode.DownArrow)){
			transform.position += new Vector3 (0, 0, -1);
		}

		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.position += new Vector3 (-1, 0, 0);
		}

		if(Input.GetKey(KeyCode.RightArrow)){
			transform.position += new Vector3 (1, 0, 0);
		}

		if(Input.GetKeyDown(KeyCode.Space)){
			DamagePopUpController.CreatFloatingText (Random.Range (1, 100).ToString(), transform);
		}

	}
}
