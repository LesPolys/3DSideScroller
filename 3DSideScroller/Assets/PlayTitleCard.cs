using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTitleCard : MonoBehaviour {

    public Hatch bossHatch;
    bool triggerOnce = false;
    GameObject titleCard;

    void Update()
    {
        if (bossHatch.hatchShut && !triggerOnce)
        {
            triggerOnce = true;
           // titleCard.GetComponent<Animator>().Play("");
            //PlayImageCard thing

        }
    }

}
