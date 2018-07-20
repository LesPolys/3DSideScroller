using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            AkSoundEngine.PostEvent("Boss_Music", gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
