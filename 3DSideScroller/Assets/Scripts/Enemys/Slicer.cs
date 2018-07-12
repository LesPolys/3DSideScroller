using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : Enemy {

    #region TARGETING
    [SerializeField]
    float searchRadius;

    [SerializeField]
    LayerMask playerMask;
    GameObject target = null;
    #endregion

    #region FLEEMECHANICS
    [SerializeField, Range(0,100)]
    float dangerHealthPerc; // the percentage of health they be under before they make theyre flee check

    [SerializeField, Range(0,100)]
    float cowardicePerc; // percentage roll check. When below the dangerhealthPerc we roll against this and see if the enemy got scared and runs away

    private bool testedCourage = false;

    public Transform[] fleePoints;
    private Transform currentFleeTarget = null;
    private float startingHealth;
    #endregion

    #region ATTACKSSTATS
    public float attackRadius;
    private float timeBetweenAttack;
    private float startTimeBetweenAttack;
    public Transform attackPos;
    #endregion

    #region STATES
    public enum SlicerStates { SEARCHING, FLEE, DEAD, SLICE, HIT, WALKING }
    public SlicerStates currState;
    #endregion

   

    protected override void Awake()
    {
        base.Awake();
        dangerHealthPerc = dangerHealthPerc * 0.01f;

        RuntimeAnimatorController ac = _animator.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == "Hit")        //If it has the same name as your clip
            {
                startStunTime = ac.animationClips[i].length;
            }

            if (ac.animationClips[i].name == "slice")        //If it has the same name as your clip
            {
               startTimeBetweenAttack = ac.animationClips[i].length + 1.0f;
            }

        }

        startingHealth = health;

       
    }


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {


        switch (currState)
        {

        
            case SlicerStates.WALKING:
                //play walking anim
                _animator.Play("Walking");
                    MoveToTarget();
                //if within range change to 

                if (Vector3.Distance(transform.position, target.transform.position) < attackRadius * 3)
                {
                    timeBetweenAttack = -1;
                    currState = SlicerStates.SLICE;
                }
                    

                break;
            case SlicerStates.FLEE:
                //play running away anim
                _animator.Play("Flee");



                if (Vector3.Distance(transform.position, currentFleeTarget.transform.position) <= 1)
                {
                    PickNewRandomPoint();
                }
                else
                {
                    transform.LookAt(new Vector3(currentFleeTarget.transform.position.x, transform.position.y, currentFleeTarget.transform.position.z));
                    _controller.Move(transform.forward * moveSpeed * Time.deltaTime);
                }


                break;
            case SlicerStates.DEAD:
                _animator.Play("Dead");
                //turn off colliders
                _controller.enabled = false;
                //wait for timer
                //fall down through floor (manual pos move)and delete

                break;
            case SlicerStates.SLICE:
                //play attack anim

                if (timeBetweenAttack <= 0)
                {     
                      
                        Collider[] playersToDamage = Physics.OverlapSphere(attackPos.position, attackRadius, playerMask);
                    
                        for (int i = 0; i < playersToDamage.Length; i++)
                        {

                            if (playersToDamage[i].GetComponent<Player>() != null)
                                playersToDamage[i].GetComponent<Player>().Damage(1);

                            if (playersToDamage[i].GetComponent<Enemy>() != null)
                                playersToDamage[i].GetComponent<Enemy>().Damage(1);


                    }

                        timeBetweenAttack = startTimeBetweenAttack;

                    _animator.Play("Slice");

                    if (playersToDamage.Length <= 0)
                    {
                        ResetTarget();
                    }

                }
                else
                {
                    _animator.Play("IDLE");
                    timeBetweenAttack -= Time.deltaTime;
                }

                break;
            case SlicerStates.HIT:
                //stun state counter
                //if health is zero or below then switch to dead
                if (health <= 0)
                {
                    isDead = true;
                    currState = SlicerStates.DEAD;
                }

                if (stunTime <= 0)
                {
                    ResetTarget();
                }
                else
                {
                    stunTime -= Time.deltaTime;
                }


            

                //when counters done check for health roll if never checked before
                    //if failed then switch to flee

                break;
            case SlicerStates.SEARCHING:

                //play idle animation
                _animator.Play("Idle");

                if (target == null)
                {
                    SearchForNewTarget();
                }
                
                if(target){
                    currState = SlicerStates.WALKING;
                }

                break;
        }

    
		
        



	}

    void ResetTarget()
    {
        target = null;
        currState = SlicerStates.SEARCHING;
    }

    void SearchForNewTarget()
    {
        Transform closestTargetHolder = null;

        Collider[] potentialTargets = Physics.OverlapSphere(transform.position, searchRadius, playerMask);

        if (potentialTargets.Length != 0)
        {
            closestTargetHolder = potentialTargets[0].transform;
        }

        for (int i = 1; i < potentialTargets.Length; i++)
        {
            if (Vector3.Distance(transform.position, potentialTargets[i].transform.position) < Vector3.Distance(transform.position, closestTargetHolder.position))
            {
                closestTargetHolder = potentialTargets[i].transform;
            }

        }

        if (closestTargetHolder != null)
        {
            target = closestTargetHolder.gameObject;
        }

    }

    void MoveToTarget()
    {
       transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        _controller.Move(transform.forward * moveSpeed * Time.deltaTime);
    }

    void TestOfCourage()
    {
      

        if(Random.Range(0,100) < cowardicePerc)
        {
            PickNewRandomPoint();
            currState = SlicerStates.FLEE;
        }

        testedCourage = true;
    }

    void PickNewRandomPoint()
    {

        currentFleeTarget = fleePoints[Random.Range(0, fleePoints.Length)];

    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);


    }

    protected override void OnDamage()
    {
        if (!isDead) {
            currState = SlicerStates.HIT;
            _animator.Play("Hit");
        }

        if (!testedCourage)
        {
           
            if (health <= startingHealth * dangerHealthPerc)
            {
                TestOfCourage();
            }
        }


    }


}
