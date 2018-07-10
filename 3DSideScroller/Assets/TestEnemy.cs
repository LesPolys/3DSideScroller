using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestEnemy : MonoBehaviour {

   
    public GameObject DamageText;
   

    [SerializeField]
    float moveSpeed = 4f;
    float health = 10;

    enum States {SEARCHING, TRACKING }
    States currState = States.SEARCHING;
    LayerMask playerMask;
    GameObject target;

    private CharacterController _controller;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        switch (currState)
        {

            case States.SEARCHING:
              //  target  Physics.OverlapSphere(transform.position, playerMask);

                break;
            case States.TRACKING:
                break;


        }
       


    }

    

    public void Damage(int damage)
    {
        health -= damage;
        GameObject popup = Instantiate(DamageText, this.transform.position, Quaternion.identity, transform);
        popup.GetComponentInChildren<TextMeshPro>().text = damage.ToString();
    }
}
