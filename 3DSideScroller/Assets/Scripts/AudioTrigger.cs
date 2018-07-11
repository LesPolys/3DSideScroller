using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour {

	public void CallAudioEvent(string s)
    {
        AkSoundEngine.PostEvent(s, gameObject);
    }
}
