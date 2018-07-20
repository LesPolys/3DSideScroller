using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseHatch : MonoBehaviour {

    public Hatch parentHatch;


    public void ShutTheHatch()
    {
        parentHatch.hatchShut = true;
    }
}
