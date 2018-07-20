using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTitleCard : MonoBehaviour {

    public Hatch bossHatch;
    bool triggerOnce = false;
    public GameObject titleCard;
    public  GameObject titleCardBG;

    void Update()
    {
        if (bossHatch.hatchShut && !triggerOnce)
        {
            print("HERE");
            triggerOnce = true;
            titleCardBG.GetComponent<Animator>().Play("StanBackground");
            //PlayImageCard thing

        }
    }

}
