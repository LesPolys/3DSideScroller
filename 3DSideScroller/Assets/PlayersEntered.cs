using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersEntered : MonoBehaviour {

    public Hatch hatch;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            hatch.ChangePlayerCount(1);
        }
  
    }

    public void OnTriggerExit(Collider other)
    {
       // print("Leave");
        if (other.gameObject.tag == "Player")
        {
            hatch.ChangePlayerCount(-1);
        }
    }


}
