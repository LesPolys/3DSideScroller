using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            this.gameObject.SetActive(false);
        }
    }
}
