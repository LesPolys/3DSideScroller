using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicerAnimationEvent : MonoBehaviour {

    public Slicer slicer;

    public void EndOfSlice()
    {
        slicer.EndOfSlice();
    }

}
