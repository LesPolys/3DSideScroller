using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestEnemy : MonoBehaviour {

    public TextMeshPro healthText;

   

    [SerializeField]
    float moveSpeed = 4f;
    float health = 10;

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

       
       // _controller.Move(transform.forward * Time.deltaTime * moveSpeed);
        healthText.text = health.ToString();

    }

    public void Damage(int damage)
    {
       // print("HELLO");
        health -= damage;
    }
}
