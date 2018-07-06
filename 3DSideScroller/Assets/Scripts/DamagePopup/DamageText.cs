using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour {

	public Animator animator;
	private TextMeshProUGUI damageText;

	void Awake(){
		damageText = animator.GetComponent<TextMeshProUGUI> ();
	}

	void Start(){
		AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo (0);
		Destroy (gameObject, clipInfo [0].clip.length);

	

	}

	public void SetText(string text){
		damageText.text = text;
	}


}
