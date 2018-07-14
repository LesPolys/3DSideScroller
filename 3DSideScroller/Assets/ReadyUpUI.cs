using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyUpUI : MonoBehaviour {


    public PlayerActions Actions { get; set; }

	public Image playerIconImage;
	public Image readyUpImage;

	[HideInInspector]
	public bool isReady = false;

	public bool hasAssignedInput = false;

	// Use this for initialization


    void OnDisable()
	{
		if (Actions != null)
		{
			Actions.Destroy();
		}
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Actions != null){
			
			hasAssignedInput = true;

			if(Actions.A){
				if(!isReady){
					isReady = true;
				}
			}

			if(Actions.B){
				if(isReady){
					isReady = false;
				}
			}
		}

	}


	

}
