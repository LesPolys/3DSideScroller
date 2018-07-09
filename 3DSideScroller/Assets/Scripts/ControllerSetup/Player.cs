
using UnityEngine;


// This is just a simple "player" script that rotates and colors a cube
// based on input read from the actions field.
//
// See comments in PlayerManager.cs for more details.
//
public class Player : MonoBehaviour
{
	public PlayerActions Actions { get; set; }

    private CharacterController _controller;


    [SerializeField]
    float moveSpeed = 4f;

    Vector3 forward, right, lastCurrent;


    private enum MoveTypes {FULLROT, LTTP, CC, EOE };
    private MoveTypes currMoveType = MoveTypes.LTTP;
    private float angleLTTP;

      void Awake()
    {
        _controller = GetComponent<CharacterController>();

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
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


        if (Actions.Start.WasPressed)
        {
            NextMoveType();
        }


		if(Actions.Move){
            Move();
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

        switch (currMoveType)
        {
          

            case MoveTypes.FULLROT:
                //Set Faceing of Player
                rotHeading = Vector3.Normalize(rightMovement + upMovement);
                transform.forward = rotHeading;
                _controller.Move(heading * Time.deltaTime * moveSpeed); // move the player
                break;

            case MoveTypes.LTTP:
                //rotHeading = Vector3.Normalize(rightMovement + upMovement);
               

                //Up
                if (Actions.Move.Y < 0 && Actions.Move.X <= 0.5 && Actions.Move.X >= -0.5 )
                {
                    rotHeading = new Vector3(0.7f, 0.0f, 0.7f);

                }

                //Down

                if (Actions.Move.Y > 0 && Actions.Move.X <= 0.5 && Actions.Move.X >= -0.5)
                {
                    rotHeading = new Vector3(-0.7f, 0.0f,- 0.7f);

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

            case MoveTypes.CC:
                rotHeading = Vector3.Normalize(rightMovement);
                transform.forward = rotHeading;
                _controller.Move(heading * Time.deltaTime * moveSpeed); // move the player
                break;

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



}


