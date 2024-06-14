using Godot;
using System;

public partial class SlimeMoving : State
{
	[ExportGroup("States")]
    [Export]
    public State Idle; ///< Reference to Idle state
    [Export]
    public State Attacking; ///< Reference to Attacking state 

	[Export]
	public float SPEED = 50; ///< Speed of the slime when it moves towards the player

	private Player _player;
	
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
	Calculate distance to player to find out whether the state should be changed.
	If the slime remains in Moving, move towards the player and play the jump/move animation.
	*/
	public override State ProcessPhysics(double delta)
	{
		if (_player is null)
		{
			return null;
		}

		Vector2 vector_to_player = (_player as CharacterBody2D).Position - Parent.Position;

		if (IsPlayerInAttackRange(vector_to_player)) // if player is within attack range, go to Attacking state
		{
			return Attacking;
		}
		if (!IsPlayerInViewrange(vector_to_player)) // if player is out of view range, go to idle state
		{
			return Idle;
		}

		// if slime remains in Moving state, move towards the player
		Parent.Velocity = vector_to_player.Normalized() * SPEED;
		Parent.MoveAndSlide();
		
		UpdateAnimations();
		
		return null;
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
	play jump/move animation. Animation name has to be constructed from the slimes properties. 
	Currently used properties: magic type. 
	If animation is already playing, the animation is NOT started again from the beginning.
	*/
	public override void UpdateAnimations()
	{
		String animation_name = (Parent as Slime).GetMagicTypeAsString() + "_jump";
		Animations.Play(animation_name);

		base.UpdateAnimations();
	}
}
