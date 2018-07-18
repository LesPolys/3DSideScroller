
using UnityEngine;
using EZCameraShake;
using System.Collections;


// This is just a simple "player" script that rotates and colors a cube
// based on input read from the actions field.
//
// See comments in PlayerManager.cs for more details.
//
public class Player : MonoBehaviour 
{
    public PlayerActions Actions { get; set; }

    private CharacterController _controller;
    private Animator _animator;


    #region Movement
    [Header("MOVEMENT")]
    [SerializeField]
    float normalMoveSpeed = 4f;
    [SerializeField]
    float blockMoveSpeed = 1f;

    float moveSpeed;
    #endregion



    Vector3 forward, right, lastCurrent;

  

   
    [SerializeField]
    private float health;

    #region Jumping/Grounding
    private Vector3 _velocity;
    [Header("JUMPING AND GROUNDING")]
    public float Gravity;
    public Transform _groundChecker;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;
    private bool _isGrounded = true;
    public float JumpHeight = 2f;
    private bool affectedByGravity = true;
    #endregion

    #region Dashing
    public float DashDistance = 5f;
    public Vector3 Drag;
    #endregion //TEST DONT REAL NEED

    #region MOVEMENTTYPES
    private enum MoveTypes {FULLROT, LTTP, CC, EOE };
    private MoveTypes currMoveType = MoveTypes.LTTP;
    private float FULLROT;
    #endregion

    public LayerMask enemyMask; //forAttacks and knowing about enemies

    #region LIGHTATTACK
  
    private float timeBetweenLightAttack; //stop button mashing
    private float startTimeBetweenLightAttack;
    [Header("LIGHT ATTACK")]
    public Transform lightAttackPos;
    public float lightAttackRange;
    [Space (20)]
    #endregion

    #region HEAVYATTACK
   
    private float timeBetweenHeavyAttack; //stop button mashing
    private float startTimeBetweenHeavyAttack;
    [Header("HEAVY ATTACK")]
    public Transform heavyAttackPos;
    public float heavyAttackRange;
    [Space(20)]
    #endregion

    #region SPECIALATTACK
 
    private float timeBetweenSpecialAttack; //stop button mashing
    private float startTimeBetweenSpecialAttack;
    [Header("SPECIAL ATACK")]
    public Transform specialAttackPos;
    public float specialAttackRange;

    private bool charging = false;
    private float chargeTime;
    public float tier1Time;
    private bool isTier1 = false;
    public float tier2Time;
    private bool isTier2 = false;
    public float tier3Time;
    private bool isTier3 = false;
    public float maxChargeTime;
    private bool forcedRelease = false;
    [Space(20)]
    #endregion

    #region BLOCK
  
    private bool attacking = false;
    private bool blocking = false;
    [Header("BLOCK")]
    public GameObject block;

    #endregion


    #region STUN
    protected float stunTime;
    protected float startStunTime ;
    bool isInHitStun = false;
    #endregion



    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController>();

        GetAnimationLengths();


        moveSpeed = normalMoveSpeed;

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    void GetAnimationLengths()
    {

        RuntimeAnimatorController ac = _animator.runtimeAnimatorController;    //Get Animator controller
        for (int i = 0; i < ac.animationClips.Length; i++)                 //For all animations
        {
            if (ac.animationClips[i].name == "Attack")        //If it has the same name as your clip
            {
                startTimeBetweenLightAttack = ac.animationClips[i].length;
            }

            if (ac.animationClips[i].name == "Heavy")        //If it has the same name as your clip
            {
                startTimeBetweenHeavyAttack = ac.animationClips[i].length;
            }

            if (ac.animationClips[i].name == "Spin")        //If it has the same name as your clip
            {
                startTimeBetweenSpecialAttack = ac.animationClips[i].length;
            }
            
            if (ac.animationClips[i].name == "Hit")        //If it has the same name as your clip
            {
                startStunTime = ac.animationClips[i].length;
            }


        }

    }



    void OnDisable()
	{
		if (Actions != null)
		{
			Actions.Destroy();
		}
	}


	void Start()
	{

	}

    // Update is called once per frame
  


    void Update()
	{
        if (stunTime <= 0)
        {
            isInHitStun = false;
            HandleInput();
        }
        else
        {
           
            stunTime -= Time.deltaTime;
        }
      
        CheckForHits();

        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore); // Check if grounded to reset velocity
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = 0f;

        #region DASHDRAG
        _velocity.x /= 1 + Drag.x * Time.deltaTime;
        _velocity.y /= 1 + Drag.y * Time.deltaTime;
        _velocity.z /= 1 + Drag.z * Time.deltaTime;
        #endregion




        if (affectedByGravity)
        {
            _velocity.y += Gravity * Time.deltaTime; //add gravity to velocity
        }




        _controller.Move(_velocity * Time.deltaTime);


        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            AttackOver();
        }

    }

    //CameraShaker.Instance.ShakeOnce(1f, 1f, 0.1f, 0.1f);
    //  Debug.Log("Dash");
    //  _velocity += Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));




    void HandleInput()
    {
        //print("yo");

        if (Actions.Start.WasPressed)
        {
            NextMoveType();
        }



        if (Actions.Move)
        {
            Move();
        }
        
        




        if (Actions.A && _isGrounded && !attacking)
        { //jump
            _velocity.y += Mathf.Sqrt(JumpHeight * -2f * Gravity);

        }

        if (Actions.LT && _isGrounded && !attacking )
        {
            blocking = true;
            block.SetActive(true);
        }
        else
        {
            blocking = false;
            block.SetActive(false) ;
        }


        if (timeBetweenLightAttack <= 0) {
           
            if (Actions.X.WasPressed && !attacking)
            {
                attacking = true;
                _animator.Play("Attack");
                timeBetweenLightAttack = startTimeBetweenLightAttack;
            
            }
        }
        else
        {
           
            timeBetweenLightAttack -= Time.deltaTime;
        }

        if (timeBetweenHeavyAttack <= 0)
        {
        
            if (Actions.Y.WasPressed && !attacking)
            {
                attacking = true;
                _animator.Play("Heavy");
                timeBetweenHeavyAttack = startTimeBetweenHeavyAttack;
            }
        }
        else
        {
           
            timeBetweenHeavyAttack -= Time.deltaTime;
        }



        if (timeBetweenSpecialAttack <= 0)
        {
            
            if (Actions.B.WasPressed && !attacking)
            {
                charging = true;
                //print("pressed");
            }

            if (charging) //charging special attack
            {
                chargeTime += Time.deltaTime;
                //print(chargeTime);

                if (chargeTime >= tier1Time)
                {
                    isTier1 = true;
                //    print("Tier1Achieved");
                    if(chargeTime >= tier2Time)
                    {
                        isTier2 = true;
                      //  print("Tier2Achieved");
                        if (chargeTime >= tier3Time)
                        {
                            isTier3 = true;
                          //  print("Tier3Achieved");
                            if (chargeTime >= maxChargeTime)
                            {
                             //   print("POWER OVERWHELMING!");
                                forcedRelease = true;
                                SpecialAttack();
                                
                            }

                        }
                    }
                }
            }


            if (Actions.B.WasReleased && !attacking)
            {
                if (forcedRelease)
                {
                  //  print("control relinquished");
                    forcedRelease = false;
                }
                else
                {
                  //  print("released");
                    SpecialAttack();
                }
               
            }


        }
        else
        {
            
            timeBetweenSpecialAttack -= Time.deltaTime;
        }


       
       


    }

    private void SpecialAttack()
    {
        chargeTime = 0;
        charging = false;
        //attacking = true;

        if (isTier3)
        {
           // print("Tier3 Attack");
            _animator.Play("Spin");
            timeBetweenSpecialAttack = startTimeBetweenSpecialAttack;

        }
        else if (isTier2)
        {
          //  print("Tier3 Attack");
            _animator.Play("Spin");
            timeBetweenSpecialAttack = startTimeBetweenSpecialAttack;

        }
        else if (isTier1)
        {
           // print("Tier3 Attack");
            _animator.Play("Spin");
            timeBetweenSpecialAttack = startTimeBetweenSpecialAttack;
        }

        //print("Tier3 Spent");
        isTier3 = false;
       // print("Tier2 Spent");
        isTier2 = false;
       // print("Tier1 Spent");
        isTier1 = false;


    }

    public void AttackOver()
    {
        attacking = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(lightAttackPos.position, lightAttackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(heavyAttackPos.position, heavyAttackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(specialAttackPos.position, specialAttackRange);

    }


    void CheckForHits()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            //print("ATTACK");
            CheckForEnemiesHit(lightAttackPos, lightAttackRange, 1);

        }
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Heavy"))
        {
            // print("HEAVY");
            CheckForEnemiesHit(heavyAttackPos, heavyAttackRange, 2);

        }
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Spin"))
        {
            //print("SPIN");
            CheckForEnemiesHit(specialAttackPos, specialAttackRange, 1);

        }
    }

    void CheckForEnemiesHit(Transform attackPos, float attackRange, int damage)
    {

        Collider[] enemiesToDamage = Physics.OverlapSphere(attackPos.position, attackRange, enemyMask);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
     
            
            if (!_isGrounded && enemiesToDamage[i].GetComponent<Enemy>().canBeJuggled)
            {

                enemiesToDamage[i].GetComponent<Enemy>().AirDamage(damage, transform);
            }
            else
            {
                enemiesToDamage[i].GetComponent<Enemy>().Damage(damage);
            }


            
        }

    }

    


    void Move()
    {
        
        //Handle Isometric Direction according to camera
        Vector3 direction = new Vector3(Actions.Move.X, 0, Actions.Move.Y);
        Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Actions.Rotate.X;
        Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Actions.Rotate.Y;

        Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
        Vector3 rotHeading = new Vector3();

        if (!_isGrounded || !attacking)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _animator.Play("Walk");
            }

            if (blocking)
            {
                if (!_isGrounded)
                {
                    moveSpeed = normalMoveSpeed;

                }
                else
                {
                    moveSpeed = blockMoveSpeed;
                }
                
            }
            else
            {
                moveSpeed = normalMoveSpeed;
            }

            switch (currMoveType)
            {

                #region FULLROT
                case MoveTypes.FULLROT:
                    //Set Faceing of Player
                    rotHeading = Vector3.Normalize(rightMovement + upMovement);
                    transform.forward = rotHeading;
                    _controller.Move(heading * Time.deltaTime * moveSpeed); // move the player
                    break;
                #endregion

                #region LTTP
                case MoveTypes.LTTP:
            


                    rightMovement = right * moveSpeed * Time.deltaTime * Mathf.RoundToInt(Actions.Move.X);
                    upMovement = forward * moveSpeed * Time.deltaTime * Mathf.RoundToInt(-1 * Actions.Move.Y);

                    rotHeading = Vector3.Normalize(rightMovement + upMovement);


                    if (rotHeading != Vector3.zero)
                    {
                        transform.forward = rotHeading;
                        lastCurrent = rotHeading;
                        _controller.Move(rotHeading * Time.deltaTime * moveSpeed); // move the player

                    }
                    else
                    {
                        _controller.Move(lastCurrent * Time.deltaTime * moveSpeed); // move the player

                    }


                    break;
                #endregion

                #region CC
                case MoveTypes.CC:

                    rotHeading = Vector3.Normalize(rightMovement);

                    if (rightMovement != Vector3.zero) // check for the case where we arnt hitting a left or right key
                    {
                        transform.forward = rotHeading;
                        lastCurrent = rotHeading;

                    }
                    _controller.Move(heading * Time.deltaTime * moveSpeed); // move the player


                    break;
                    #endregion
            }

         

        }
       


      


    }

    void NextMoveType()
    {
        currMoveType++;
        print(currMoveType);
        if (currMoveType == MoveTypes.EOE ){

            currMoveType = MoveTypes.FULLROT;
        }

    }

    public void Hit(int damage, bool unblockable)
    {

        if (unblockable)
        {
            Damage(damage);
        }
        else
        {
            if (blocking)
            {
                Damage(0);
                //print("BLOCKED BITCH");
                //play block effects and sounds
                AkSoundEngine.PostEvent("Player_Block", gameObject);
            }
            else
            {
                Damage(damage);
            }
        }

    }


    public void Damage(int damage)
    {

        if(damage > 0 && !isInHitStun)
        {
            _animator.Play("Hit",-1,0);
            isInHitStun = true;
            stunTime = startStunTime;
            health -= damage;
            //print("OUCH: " + damage + "Im at: " + health);
            //play damage effect and sound
            CameraShaker.Instance.ShakeOnce(0.5f, 1f, 0.1f, 0.1f);
            AkSoundEngine.PostEvent("Player_Damage", gameObject);
            if (health <= 0)
            {

            }
        }

      
        
    }



}


