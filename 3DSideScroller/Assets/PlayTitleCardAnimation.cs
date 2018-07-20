using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTitleCardAnimation : MonoBehaviour {

    public GameObject image;

    public void PlayTitleCardSlide()
    {
        image.GetComponent<Animator>().Play("IAmStan");
    }

}
