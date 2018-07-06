using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpController : MonoBehaviour {

	private static DamageText popupText;
	public static GameObject canvas;

	public static void Initialize(){
		canvas = GameObject.Find ("Canvas"); // tis is some slow ass shit
		if(!popupText)
			popupText = Resources.Load<DamageText> ("Prefabs/PopUpTest/PopUpTextParent"); // change this later, having a hard coded string location reference is a real shite idea Logan, use some damn variable to hold the location if were taking this route
	}

	public static void CreatFloatingText (string text, Transform location){
		DamageText instance = Instantiate (popupText);


		Vector2 screenPos = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-0.5f,0.5f),location.position.z ));





		instance.transform.SetParent (canvas.transform, false); //always do this for UI elements 
		instance.transform.position = screenPos;
		instance.SetText (text);

	}


}
