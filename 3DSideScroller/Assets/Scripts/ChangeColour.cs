using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColour : MonoBehaviour {
	Image readyUpButton;


	void Awake(){
		readyUpButton = GetComponent<Image>();
	}

	public void SwapColour(){
		
		if(readyUpButton.color == Color.red){
				readyUpButton.color = Color.green;

		}

		if(readyUpButton.color == Color.green){
				readyUpButton.color = Color.red;

		}


	}

	void ResetColour(){
		readyUpButton.color = Color.white;

	}


}
