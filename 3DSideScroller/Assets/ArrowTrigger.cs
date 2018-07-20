using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrigger : MonoBehaviour {


    public GameObject arrowImage;
    public Hatch[] hatches;
    bool animPlayed = false;

	void Awake()
    {
       
    }
	
	// Update is called once per frame
	void Update () {

        if (CheckIfAllHatchesClosed() && !animPlayed)
        {
            StartCoroutine(PlayArrowAnim());
        }

	}

    IEnumerator PlayArrowAnim()
    {

        arrowImage.SetActive(true);
        arrowImage.GetComponent<Animator>().Play("Next");
        yield return new WaitForSeconds(5);
        arrowImage.SetActive(false);
        animPlayed = true;
        yield break;
    }

    public bool CheckIfAllHatchesClosed()
    {
        foreach (Hatch hatch in hatches)
        {
            if (hatch.hatchShut == false)
            {
                return false;
            }
        }
        return true;
    }

}
