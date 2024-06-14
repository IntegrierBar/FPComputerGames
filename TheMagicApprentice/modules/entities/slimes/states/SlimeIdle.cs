using Godot;
using System;

public partial class SlimeIdle : State
{
	[ExportGroup("States")]
    [Export]
    public State Moving; ///< Reference to Moving state
    [Export]
    public State Attacking; ///< Reference to Attacking state 

	[Export]
	public float SPEED = 20; ///< Speed of the slime when it is idle

	[ExportGroup("Animation Duration")]
	[Export]
	public double IdleAnimationDuration; ///< Duration of the idle animation
	[Export]
	public double JumpAnimationDuration; ///< duration of the jump/move animation

	private Player _player;

	private double _timeLeft = 0.0; ///< time left in which the slime either remains in one position, or jumps around randomly
	private bool _idleAtSamePosition = true; ///< is true when the slime stays in one position and false for random walk, changes from true to false whenever timeLeft reaches zero
	// Setting _idleatSamePosition to true means it will get set to false in the first physics process 
	// and the slime starts jumping around.

	/**
    Set player. 
	*/
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player") as Player;
		if (_player is null)
		{
			GD.Print("No player found!");
		}
	}

	/**
    First, if there is a player, check if the slime is close enough to change state. 
	If not, reduce timeLeft. If timeLeft reaches zero, the slime changes from staying idle in one place 
	to randomly walking in one direction or the other way around. This is done by calling the 
	ChangeRandomWalk function.
	Afterwards, move_and_slide is called so that the slime can move.
    */
	public override State ProcessPhysics(double delta)
	{
		if (_player is null)
		{
			return null;
		}

		Vector2 vector_to_player = (_player as CharacterBody2D).Position - Parent.Position;

		if (IsPlayerInAttackRange(vector_to_player))
		{
			return Attacking;
		}
		if (IsPlayerInViewrange(vector_to_player))
		{
			return Moving;
		}

		_timeLeft -= delta;
		if (_timeLeft <= 0.0)
		{
			Vector2 new_direction = ChangeRandomWalk();
			Parent.Velocity = new_direction * SPEED; // update the slimes velocity
		}
		Parent.MoveAndSlide();
		UpdateAnimations();
		return null;
	}

	/**
    Changes action of the slime from staying idle in one place to randomly walking in one direction 
	or the other way around. 
	Change _idleAtSamePsoition first to update what the slime is doing now.
	Generate an integer between 0 and 5 that is later multiplied by the animation duration to ensure 
	that the changes in movements of the idle slime do not happen in the middle of animations.
	Set timeLeft in the current state and generate a reandom directio if the slime should move.
	Return the direction or (0,0) if the slime stays at the same position. 
    */
	public Vector2 ChangeRandomWalk()
	{
		var random = new RandomNumberGenerator(); // necessary for generating some random numbers

		uint times = GD.Randi() % 5; // using an integer to ensure that changes of idle state type don't happen during an animation
		_idleAtSamePosition = !_idleAtSamePosition; // change idle state type, true means idle stationary, false means walking in a direction

		if (_idleAtSamePosition) // idle stationary
		{
			_timeLeft = (double)times * IdleAnimationDuration; // timeLeft is always a multiple of the animation duration
			return new Vector2(0.0f, 0.0f); // direction is set to zero so that the slime does not move
		}
		else
		{
			_timeLeft = (double)times * JumpAnimationDuration; // timeLeft is always a multiple of the animation duration
			return new Vector2(random.RandfRange(-1, 1), random.RandfRange(-1, 1)).Normalized(); // set a random direction and normalise
		}
	}

	/**
    Updates the slimes animation based on its current activity, idle stationary or walking in a random direction.
    */
    public override void UpdateAnimations()
    {
		if (_idleAtSamePosition)
		{
			String animation_name = (Parent as Slime).GetMagicTypeAsString() + "_idle";
			Animations.Play(animation_name);
		}
		else
		{
			String animation_name = (Parent as Slime).GetMagicTypeAsString() + "_jump";
			Animations.Play(animation_name);
		}
        base.UpdateAnimations();
    }

	/**
	Calculates vector from slime to player and finds out if the player is within the slimes attack range
	*/
	private bool IsPlayerInAttackRange(Vector2 vector_to_player)
	{
		return vector_to_player.Length() <= (Parent as Slime).GetAttackRangeF();
	}

	/**
	Calculates vector from slime to player and finds out if the player is within the slimes view range
	*/
	private bool IsPlayerInViewrange(Vector2 vector_to_player)
	{
		return vector_to_player.Length() <= (Parent as Slime).GetViewRange();
	}

	/**
    Getter for time left. Currently not used.
	*/
	public double GetTimeLeft()
	{
		return _timeLeft;
	}
}
