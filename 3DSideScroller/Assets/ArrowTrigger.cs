using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrigger : MonoBehaviour {


    public GameObject arrowImage;
    public Hatch[] hatches;
    bool animPlayed = false;

	void Awake()
    {
        arrowImage.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (CheckIfAllHatchesClosed() && !animPlayed)
        {
            
        }

	}

    IEnumerator PlayArrowAnim()
    {
        arrowImage.SetActive(true);
        arrowImage.GetComponent<Animator>().Play("Next");
        yield return new WaitForSeconds(2);
        arrowImage.SetActive(false);
        yield break;
    }

    public bool CheckIfAllHatchesClosed()
    {
        foreach (Hatch hatch in hatches)
        {
            if (hatch.hatchOpen)
            {
                return false;
            }
        }
        return true;
    }

}
