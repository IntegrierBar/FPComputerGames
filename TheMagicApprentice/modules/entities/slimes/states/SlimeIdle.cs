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

	private Node _player;

	private double _timeLeft; ///< time left in which the slime either remains in one position, or jumps around randomly
	private bool _idleAtSamePosition; ///< is true when the slime stays in one position and false for random walk, changes from true to false whenever timeLeft reaches zero
	private Vector2 _direction; ///< direction in which the slime moves during its random walk
	

	/**
    Set player. Also set timeLeft to zero so that a new idle loop starts. 
	Setting _idleatSamePosition to true means it will get set to false in the first physics process 
	and the slime starts jumping around. 
    */
	public override void _Ready()
	{
		_player = GetTree().GetFirstNodeInGroup("player");
		if (_player is null)
		{
			GD.Print("No player found!");
		}

		_timeLeft = 0.0;
		_idleAtSamePosition = true;
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
		if (_player is not null)
		{
			Vector2 vector_to_player = (_player as CharacterBody2D).Position - Parent.Position;
			if (vector_to_player.Length() <= (Parent as Slime).GetAttackRangeF())
			{
				return Attacking;
			}
			if (vector_to_player.Length() <= (Parent as Slime).GetViewRange())
			{
				return Moving;
			}
		}

		_timeLeft -= delta;
		if (_timeLeft <= 0.0)
		{
			ChangeRandomWalk();	
		}
		Parent.MoveAndSlide();
		return null;
	}

	/**
    Changes action of the slime from staying idle in one place to randomly walking in one direction 
	or the other way around. 
	Change _idleAtSamePsoition first to update what the slime is doing now.
	Generate an integer between 0 and 5 that is later multiplied by the animation duration to ensure 
	that the changes in movements of the idle slime do not happen in the middle of animations.
	Set timeLeft in the current state and set the direction accordingly, (0,0) if the slime stays at 
	the same position. 
	Afterwards, update the slimes velocity.
    */
	private void ChangeRandomWalk()
	{
		var random = new RandomNumberGenerator(); // necessary for generating some random numbers

		uint times = GD.Randi() % 5; // using an integer to ensure that changes of idle state type don't happen during an animation
		_idleAtSamePosition = !_idleAtSamePosition; // change idle state type, true means idle stationary, false means walking in a direction

		if (_idleAtSamePosition) // idle stationary
		{
			_timeLeft = (double)times * IdleAnimationDuration; // timeLeft is always a multiple of the animation duration
			_direction = new Vector2(0.0f, 0.0f); // direction is set to zero so that the slime does not move
		}
		else
		{
			_timeLeft = (double)times * JumpAnimationDuration; // timeLeft is always a multiple of the animation duration
			_direction = new Vector2(random.RandfRange(-1, 1), random.RandfRange(-1, 1)).Normalized(); // set a random direction and normalise
		}
		Parent.Velocity = _direction * SPEED; // update the slimes velocity
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
}
