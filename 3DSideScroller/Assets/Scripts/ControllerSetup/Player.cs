
using UnityEngine;
using EZCameraShake;


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


    [SerializeField]
    float moveSpeed = 4f;

    Vector3 forward, right, lastCurrent;

    private bool attacking = false;


   
    [SerializeField]
    private float health;

    #region Jumping/Grounding
    private Vector3 _velocity;
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
    public Transform lightAttackPos;
    public float lightAttackRange;
    #endregion

    #region HEAVYATTACK
    private float timeBetweenHeavyAttack; //stop button mashing
    private float startTimeBetweenHeavyAttack;
    public Transform heavyAttackPos;
    public float heavyAttackRange;
    #endregion

    #region SPECIALATTACK
    private float timeBetweenSpecialAttack; //stop button mashing
    private float startTimeBetweenSpecialAttack;
    public Transform specialAttackPos;
    public float specialAttackRange;
    #endregion

    #region BLOCK
    #endregion


    

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController>();

        GetAnimationLengths();




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

       
        HandleInput();
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


    }

    //CameraShaker.Instance.ShakeOnce(1f, 1f, 0.1f, 0.1f);
    //  Debug.Log("Dash");
    //  _velocity += Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * Drag.x + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime)));

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
            CheckForEnemiesHit(heavyAttackPos, heavyAttackRange, 1);

        }
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Spin"))
        {
            //print("SPIN");
            CheckForEnemiesHit(specialAttackPos, specialAttackRange, 1);

        }
    }


    void HandleInput()
    {

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
                attacking = true;
                _animator.Play("Spin");
                timeBetweenSpecialAttack = startTimeBetweenSpecialAttack;
            }
        }
        else
        {
            
            timeBetweenSpecialAttack -= Time.deltaTime;
        }


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


    void CheckForEnemiesHit(Transform attackPos, float attackRange, int damage)
    {

        Collider[] enemiesToDamage = Physics.OverlapSphere(attackPos.position, attackRange, enemyMask);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
     
            CameraShaker.Instance.ShakeOnce(0.5f, 1f, 0.1f, 0.1f);
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
                    //rotHeading = Vector3.Normalize(rightMovement + upMovement);

                    //print(Actions.Move.X);


                    //Up
                    if (Actions.Move.Y < 0 && Actions.Move.X <= 0.5 && Actions.Move.X >= -0.5)
                    {
                        rotHeading = new Vector3(0.7f, 0.0f, 0.7f);

                    }

                    //Down

                    if (Actions.Move.Y > 0 && Actions.Move.X <= 0.5 && Actions.Move.X >= -0.5)
                    {
                        rotHeading = new Vector3(-0.7f, 0.0f, -0.7f);

                    }

                    //Left

                    if (Actions.Move.X < 0 && Actions.Move.Y <= 0.5 && Actions.Move.Y >= -0.5)
                    {
                        rotHeading = new Vector3(-0.7f, 0.0f, 0.7f);

                    }

                    //Right

                    if (Actions.Move.X > 0 && Actions.Move.Y <= 0.5 && Actions.Move.Y >= -0.5)
                    {
                        rotHeading = new Vector3(0.7f, 0.0f, -0.7f);

                    }

                    //UPLEFT

                    if (Actions.Move.X < 0 && Actions.Move.Y <= -0.20 && Actions.Move.Y >= -0.75)
                    {
                        rotHeading = new Vector3(0.1f, 0.0f, 1.0f);

                    }

                    //UPRIGHT

                    if (Actions.Move.X > 0 && Actions.Move.Y <= -0.20 && Actions.Move.Y >= -0.75)
                    {
                        rotHeading = new Vector3(1.0f, 0.0f, 0.1f);

                    }

                    //DOWNLEFT

                    if (Actions.Move.X < 0 && Actions.Move.Y >= 0.20 && Actions.Move.Y <= 0.75)
                    {

                        rotHeading = new Vector3(-1.0f, 0.0f, 0.1f);
                    }

                    //DOWNRIGHT

                    if (Actions.Move.X > 0 && Actions.Move.Y >= 0.20 && Actions.Move.Y <= 0.75)
                    {
                        rotHeading = new Vector3(0.1f, 0.0f, -1.0f);

                    }

                    //Up
                    if (Actions.Move.Y < 0 && Actions.Move.X <= 0.5 && Actions.Move.X >= -0.5)
                    {
                        rotHeading = new Vector3(0.7f, 0.0f, 0.7f);

                    }

                    //Down

                    if (Actions.Move.Y > 0 && Actions.Move.X <= 0.5 && Actions.Move.X >= -0.5)
                    {
                        rotHeading = new Vector3(-0.7f, 0.0f, -0.7f);

                    }

                    //Left

                    if (Actions.Move.X < 0 && Actions.Move.Y <= 0.5 && Actions.Move.Y >= -0.5)
                    {
                        rotHeading = new Vector3(-0.7f, 0.0f, 0.7f);

                    }

                    //Right

                    if (Actions.Move.X > 0 && Actions.Move.Y <= 0.5 && Actions.Move.Y >= -0.5)
                    {
                        rotHeading = new Vector3(0.7f, 0.0f, -0.7f);

                    }

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

    public void Hit(bool unblockable)
    {

        if (unblockable)
        {

        }
        else
        {

        }

    }


    public void Damage(int damage)
    {
        health -= damage;

        

        if(health <= 0)
        {

        }
        
    }



}


