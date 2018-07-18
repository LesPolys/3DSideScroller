using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpDamage : MonoBehaviour {

    public Animator _animator;


	void Awake()
    {
        AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);

    }
}
