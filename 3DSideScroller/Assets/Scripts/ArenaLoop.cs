using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;

public class ArenaLoop : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TestEnemy")
        {
   
                ExampleCharacterController cc = other.GetComponent<ExampleCharacterController>();
                if (cc)
                {
                Vector3 telePos = new Vector3(other.transform.position.x + 20, other.transform.position.y, other.transform.position.z);
                    cc.Motor.SetPositionAndRotation(telePos, other.transform.rotation);
                }


        }
    }
}
 