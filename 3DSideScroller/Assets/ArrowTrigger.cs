using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrigger : MonoBehaviour {


    public GameObject arrowIMage;
    public Hatch[] hatches;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        CheckIfAllHatchesClosed();

	}

    public void CheckIfAllHatchesClosed()
    {

    }

}
