using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using TMPro;

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
    public enum SlicerStates { SEARCHING, FLEE, DEAD, SLICE, HIT, KNOCKUP, WALKING, IDLE }
    public SlicerStates currState;
    #endregion

    #region Jumping/Grounding
    private Vector3 _velocity;
    public float Gravity;
    public Transform _groundChecker;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;
    private bool _isGrounded = true;
    private bool affectedByGravity = true;
    #endregion

    public GameObject hellHoleParticle;
    public Transform hellholespawnpoint;

    public TextMeshPro damageText;


    float kuHeight;
    public float knockUpSpeed;
    public bool knockUpCoRun = false;
  

    private bool dissapearing = false;

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

            if (ac.animationClips[i].name == "Idle")        //If it has the same name as your clip
            {
               startTimeBetweenAttack = ac.animationClips[i].length * 2;
            }

        }

        startingHealth = health;

       
    }


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {

        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore); // Check if grounded to reset velocity

        if (useGravity)
        {
            
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = 0f;
            }

            if (affectedByGravity)
            {
                _velocity.y += Gravity * Time.deltaTime; //add gravity to velocity
            }

            _controller.Move(_velocity * Time.deltaTime);
        }

        


        CheckForHits();

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
                useGravity = true;
                if (!dissapearing && _isGrounded)
                {
                    _controller.enabled = false;
                    StartCoroutine(Dissapear());
                }
            
                //wait for timer
                //fall down through floor (manual pos move)and delete

                break;
            case SlicerStates.SLICE:
                //play attack anim

               

                if (timeBetweenAttack <= 0)
                {

                    _animator.Play("Slice");

                }
              

                //print(timeBetweenAttack);


                break;

            case SlicerStates.IDLE:
                //play attack anim

                if (timeBetweenAttack <= 0)
                {

                    ResetTarget();

                }
                else
                {
                    _animator.Play("Idle");
                    timeBetweenAttack -= Time.deltaTime;
                }


              
                

                //print(timeBetweenAttack);


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
                    // print("HIT");

                    if (!knockUpCoRun && _isGrounded)
                    {
                        knockedUp = false;
                    }


                    if (!knockedUp && _isGrounded && !knockUpCoRun) { 
                        ResetTarget();
                    }
                }
                else
                {
                    stunTime -= Time.deltaTime;
                }
                break;

            case SlicerStates.KNOCKUP:
                //stun state counter
                //if health is zero or below then switch to dead
                //  print("We KnockedUp");
               
                
                if (health <= 0)
                {

                    isDead = true;
                    currState = SlicerStates.DEAD;
                }

                if (!knockUpCoRun)
                {
                    StartCoroutine(KnockUpCountDown());
                }

                currState = SlicerStates.HIT;
                
                
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

    IEnumerator KnockUpCountDown()
    {
        knockedUp = true;
        knockUpCoRun = true;
       while(knockUpstunTime > 0) {

            if (transform.position.y > kuHeight)
            {
                knockUpstunTime -= Time.deltaTime;
                yield return null;
            }
            else
            {

                _controller.Move(transform.up * knockUpSpeed * Time.deltaTime);
            }

            

            
            //print(knockUpstunTime);

        }
        useGravity = true;
        knockUpCoRun = false;
        yield break;


      

    }

    public void EndOfSlice()
    {
        timeBetweenAttack = startTimeBetweenAttack;
        currState = SlicerStates.IDLE;
    }

    IEnumerator Dissapear()
    {

        dissapearing = true;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        yield return new WaitForSeconds(1f);
        Instantiate(hellHoleParticle, hellholespawnpoint.position, Quaternion.identity, transform);
        yield return new WaitForSeconds(3f);
   

        while (elapsedTime < 5)
        {
           transform.position = Vector3.Lerp(startPos, new Vector3(transform.position.x, -20f, transform.position.z), (elapsedTime/5));
            elapsedTime += Time.deltaTime;
            yield return null;

            if(transform.position.y < -10f)
            {
                Destroy(gameObject);
            }

            dissapearing = false;
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

    protected override void OnDamage(int damage )
    {
        if (!isDead && currState != SlicerStates.HIT) {

           
            stunTime = startStunTime;
            currState = SlicerStates.HIT;

           
          
            //PlayMode.StopAll
            health -= damage;
            CreateDamageText(damage);
            CameraShaker.Instance.ShakeOnce(1f, 1f, 0.1f, 0.1f);
            _animator.Play("Hit",-1,0f);
        }

        if (!testedCourage)
        {
           
            if (health <= startingHealth * dangerHealthPerc)
            {
                TestOfCourage();
            }
        }


    }



    protected override void OnKnockUpDamage(int damage, float stunLenth,  float knockUpHeight)
    {
        if (!isDead && currState != SlicerStates.KNOCKUP)
        {
            //print("KNOCKUPHIT");
         
            canBeJuggled = false;
            useGravity = false;


            if (knockUpHeight == 0)
            {
                print("KNockupbonus");
                knockUpstunTime += stunLenth;
                // kuHeight = knockUpHeight;
                currState = SlicerStates.KNOCKUP;
            }
            else
            {
                knockUpstunTime = stunLenth;
                kuHeight = knockUpHeight;
                currState = SlicerStates.KNOCKUP;
            }
         


            //PlayMode.StopAll
            health -= damage;
            CreateDamageText(damage);
            CameraShaker.Instance.ShakeOnce(1f, 1f, 0.1f, 0.1f);
            _animator.Play("Hit", -1, 0f);


            /*
 if (canBeJuggled)
    {
        //print("cjknbf");
        transform.position += transform.up* knockUpForce * Time.deltaTime;
    }
    */
        }


    }

    

    void CreateDamageText(int damage)
    {
        TextMeshPro hitText = Instantiate(damageText, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), Quaternion.identity, transform);
        hitText.text = damage.ToString();
    }

    void CheckForHits()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Slice"))
        {
            //print("ATTACK");
            CheckForHit(attackPos, attackRadius, 1);

        }
      
    }

    void CheckForHit(Transform attackPos, float attackRange, int damage)
    {

        Collider[] playersToDamage = Physics.OverlapSphere(attackPos.position, attackRadius, playerMask);

        for (int i = 0; i < playersToDamage.Length; i++)
        {
          
            if (playersToDamage[i].GetComponent<Player>() != null)
            {
             
                playersToDamage[i].GetComponent<Player>().Hit(1, false);
            }
            


        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (playersToDamage.Length <= 0)
            {
                ResetTarget();
            }

        }
         


    }


}
