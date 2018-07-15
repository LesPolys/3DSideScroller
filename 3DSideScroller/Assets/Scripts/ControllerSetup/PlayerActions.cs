
using InControl;


public class PlayerActions : PlayerActionSet
{
    //use this class to create Bindings for your various buttons on the controller that you can call by name


	public PlayerAction A;
	public PlayerAction B;
	public PlayerAction X;
	public PlayerAction Y;
	public PlayerAction Left;
	public PlayerAction Right;
	public PlayerAction Up;
	public PlayerAction Down;
   
    public PlayerTwoAxisAction Move;
	public PlayerTwoAxisAction Rotate;

    public PlayerAction Start;


    public PlayerActions()
	{
		A = CreatePlayerAction( "A" );
		B = CreatePlayerAction( "B" );
		X = CreatePlayerAction( "X" );
		Y = CreatePlayerAction( "Y" );
		Left = CreatePlayerAction( "Left" );
		Right = CreatePlayerAction( "Right" );
		Up = CreatePlayerAction( "Up" );
		Down = CreatePlayerAction( "Down" );
        Move = CreateTwoAxisPlayerAction(Left, Right, Up, Down);
		Rotate = CreateTwoAxisPlayerAction( Left, Right, Down, Up );

        Start = CreatePlayerAction("Start");

	}


	public static PlayerActions CreateWithKeyboardBindings()
	{
		var actions = new PlayerActions();

		actions.A.AddDefaultBinding( Key.A );
		actions.B.AddDefaultBinding( Key.S );
		actions.X.AddDefaultBinding( Key.D );
		actions.Y.AddDefaultBinding( Key.F );

		actions.Up.AddDefaultBinding( Key.UpArrow );
		actions.Down.AddDefaultBinding( Key.DownArrow );
		actions.Left.AddDefaultBinding( Key.LeftArrow );
		actions.Right.AddDefaultBinding( Key.RightArrow );

        actions.Start.AddDefaultBinding(Key.Tab);

		return actions;
	}


	public static PlayerActions CreateWithJoystickBindings()
	{
		var actions = new PlayerActions();

		actions.A.AddDefaultBinding( InputControlType.Action1 );
		actions.B.AddDefaultBinding( InputControlType.Action2 );
		actions.X.AddDefaultBinding( InputControlType.Action3 );
		actions.Y.AddDefaultBinding( InputControlType.Action4 );

		actions.Up.AddDefaultBinding( InputControlType.LeftStickUp );
		actions.Down.AddDefaultBinding( InputControlType.LeftStickDown );
		actions.Left.AddDefaultBinding( InputControlType.LeftStickLeft );
		actions.Right.AddDefaultBinding( InputControlType.LeftStickRight );


        actions.Start.AddDefaultBinding(InputControlType.Command);

        return actions;
	}

public static PlayerActions CreateEmptyBinding(){
	var actions = new PlayerActions();
	return actions;

}

}


