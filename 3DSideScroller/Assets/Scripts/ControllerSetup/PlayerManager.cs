
using System.Collections.Generic;
using InControl;
using UnityEngine;


// This example iterates on the basic multiplayer example by using action sets with
// bindings to support both joystick and keyboard players. It would be a good idea
// to understand the basic multiplayer example first before looking a this one.
//
public class PlayerManager : MonoBehaviour
{

	private enum PlayerManagerStates{ISREADYINGPLAYERS, PLAYERSAREREADY  };
	private PlayerManagerStates currentPLayerManagerState = PlayerManagerStates.ISREADYINGPLAYERS;
	
	[SerializeField]
	private ReadyUpUI[] readyUpSpots;
	private int readyAssignedIndex = 0;
	int readyPlayerCount = 0; // how many players are ready from the list


	public GameObject playerPrefab;

	const int maxPlayers = 4;

    public List<Transform> playerPositions;

	List<Player> players = new List<Player>( maxPlayers );

	PlayerActions keyboardListener;
	PlayerActions joystickListener;


	void OnEnable()
	{
		InputManager.OnDeviceDetached += OnDeviceDetached;
		keyboardListener = PlayerActions.CreateWithKeyboardBindings();
		joystickListener = PlayerActions.CreateWithJoystickBindings();
	}


	void OnDisable()
	{
		InputManager.OnDeviceDetached -= OnDeviceDetached;
		joystickListener.Destroy();
		keyboardListener.Destroy();
	}


	void Update()
	{

		//if state bool is set to UI Mode, then when the button is pressed we will assign control of the UI element to the specific input device, 
		//Sonce the ready up buttons have all been pressed then it runs the create player section for each player

		switch(currentPLayerManagerState){

			case PlayerManagerStates.ISREADYINGPLAYERS:

				if(readyAssignedIndex < maxPlayers){
					if (JoinButtonWasPressedOnListener( joystickListener ))
					{
						var inputDevice = InputManager.ActiveDevice;

						if (ThereIsNoReadyUpUsingJoystick( inputDevice )) //make a copy of this function that checks against the UI element list rather than the player
						{
							readyUpSpots[readyAssignedIndex].Actions.Device = inputDevice;
							readyAssignedIndex++;
						}
					}

					if (JoinButtonWasPressedOnListener( keyboardListener ))
					{
						if (ThereIsNoReadyUpUsingKeyboard()) //make a copy of this function that checks against the UI element list rather than the player
						{
							print("EYLMAO");
							readyUpSpots[readyAssignedIndex].Actions = keyboardListener;
							readyAssignedIndex++;
						}
					}
				}

				if(readyAssignedIndex > maxPlayers){
					readyAssignedIndex = maxPlayers;
				}


				for(int i = 0; i < readyUpSpots.Length; i++){
					if(readyUpSpots[i].isReady){
						readyPlayerCount++; // for easch player count if there ready
					}
				}

				if(players.Count > 1 && readyPlayerCount == players.Count){ //are all players ready
					for(int i = 0; i < readyUpSpots.Length; i++){
						if(readyUpSpots[i].Actions == keyboardListener){//if it is a keyboard binding then we add the player
							CreatePlayer(null);
						}else if(readyUpSpots[i].Actions.Device != null ){ // if we have a device bound which only happens with joysticks or gamepads so we can safely assume we can bind it to the new player
							CreatePlayer(readyUpSpots[i].Actions.Device);
						}
					}
					currentPLayerManagerState = PlayerManagerStates.PLAYERSAREREADY;
				}else{
					readyPlayerCount = 0;
				}

				break;

			case PlayerManagerStates.PLAYERSAREREADY:
				
				//Move the UI Elements out of the way so we can see the GameScreen
						
				break;


		}
/* 
	if (JoinButtonWasPressedOnListener( joystickListener ))
		{
			var inputDevice = InputManager.ActiveDevice;

			if (ThereIsNoPlayerUsingJoystick( inputDevice ))
			{
				//CreatePlayer( inputDevice ); // change this to look at ui element first
			}
		}

		if (JoinButtonWasPressedOnListener( keyboardListener ))
		{
			if (ThereIsNoPlayerUsingKeyboard())
			{
				//	CreatePlayer( null );
			}
		}*/


		
	}


	bool JoinButtonWasPressedOnListener( PlayerActions actions )
	{
		return actions.A.WasPressed || actions.B.WasPressed || actions.X.WasPressed || actions.Y.WasPressed;
	}



	Player FindPlayerUsingJoystick( InputDevice inputDevice )
	{
		var playerCount = players.Count;
		for (var i = 0; i < playerCount; i++)
		{
			var player = players[i];
			if (player.Actions.Device == inputDevice)
			{
				return player;
			}
		}

		return null;
	}

	
	ReadyUpUI FindReadyUpUsingJoystick( InputDevice inputDevice )
	{
		var readyCount = readyUpSpots.Length;
		for (var i = 0; i < readyCount; i++)
		{
			var readyUp = readyUpSpots[i];
			if (readyUp.Actions.Device == inputDevice)
			{
				return readyUp;
			}
		}

		return null;
	}

	bool ThereIsNoPlayerUsingJoystick( InputDevice inputDevice )
	{
		return FindPlayerUsingJoystick( inputDevice ) == null;
	}

	bool ThereIsNoReadyUpUsingJoystick( InputDevice inputDevice )
	{
		return FindReadyUpUsingJoystick( inputDevice ) == null;
	}


	Player FindPlayerUsingKeyboard()
	{
		var playerCount = players.Count;
		for (var i = 0; i < playerCount; i++)
		{
			var player = players[i];
			if (player.Actions == keyboardListener)
			{
				return player;
			}
		}

		return null;
	}

		ReadyUpUI FindReadyUpUsingKeyboard()
	{
		var	readyCount = readyUpSpots.Length;
		for (var i = 0; i < readyCount; i++)
		{
			var readyUp = readyUpSpots[i];
			if (readyUp.Actions == keyboardListener)
			{
				return readyUp;
			}
		}

		return null;
	}


	bool ThereIsNoPlayerUsingKeyboard()
	{
		return FindPlayerUsingKeyboard() == null;
	}

	bool ThereIsNoReadyUpUsingKeyboard()
	{
		return FindReadyUpUsingKeyboard() == null;
	}


	void OnDeviceDetached( InputDevice inputDevice )
	{
		var player = FindPlayerUsingJoystick( inputDevice );
		if (player != null)
		{
			RemovePlayer( player );
		}
	}


	Player CreatePlayer( InputDevice inputDevice )
	{
		if (players.Count < maxPlayers)
		{
			// Pop a position off the list. We'll add it back if the player is removed.
			var playerPosition = playerPositions[0].position;
			playerPositions.RemoveAt( 0 );

			var gameObject = (GameObject) Instantiate( playerPrefab, playerPosition, Quaternion.identity );
			var player = gameObject.GetComponent<Player>();

			if (inputDevice == null)
			{
				// We could create a new instance, but might as well reuse the one we have
				// and it lets us easily find the keyboard player.
				player.Actions = keyboardListener;
			}
			else
			{
				// Create a new instance and specifically set it to listen to the
				// given input device (joystick).
				var actions = PlayerActions.CreateWithJoystickBindings();
				actions.Device = inputDevice;

				player.Actions = actions;
			}

			players.Add( player );

			return player;
		}

		return null;
	}





	void RemovePlayer( Player player )
	{
		//playerPositions.Insert( 0, player.transform );
		players.Remove( player );
		player.Actions = null;
		Destroy( player.gameObject );
	}

    /*
	void OnGUI()
	{
		const float h = 22.0f;
		var y = 10.0f;

		GUI.Label( new Rect( 10, y, 300, y + h ), "Active players: " + players.Count + "/" + maxPlayers );
		y += h;

		if (players.Count < maxPlayers)
		{
			GUI.Label( new Rect( 10, y, 300, y + h ), "Press a button or a/s/d/f key to join!" );
			y += h;
		}
	}*/
}
