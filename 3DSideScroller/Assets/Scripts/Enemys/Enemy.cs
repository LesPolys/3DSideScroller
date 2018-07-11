using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected float health;


    protected float stunTime;
    protected float startStunTime = 0;

    protected Animator _animator;
    protected CharacterController _controller;



    protected bool isDead = false;

    //Static variables are shared across all instances
    //of a class.
    public static int enemyCount = 0;

   protected virtual void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    public Enemy()
    {
        //Increment the static variable to know how many
        //objects of this class have been created.
        enemyCount++;
    }

    void OnDestroy() 
    {
        enemyCount--;
    }


    public void Damage(int damage)
    {
        stunTime = startStunTime;
        health -= damage;
        OnDamage();
    }

    protected virtual void OnDamage()
    {

    }

}
