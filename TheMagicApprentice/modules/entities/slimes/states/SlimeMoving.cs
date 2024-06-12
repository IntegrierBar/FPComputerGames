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
		if (_player is not null)
		{
			Vector2 vector_to_player = (_player as CharacterBody2D).Position - Parent.Position;
			if (vector_to_player.Length() <= (Parent as Slime).GetAttackRangeF()) // if player is within attack range, go to Attacking state
			{
				return Attacking;
			}
			if (vector_to_player.Length() > (Parent as Slime).GetViewRange()) // if player is out of view range, go to idle state
			{
				return Idle;
			}

			// if slime remains in Moving state, move towards the player
			Parent.Velocity = vector_to_player.Normalized() * SPEED;
			Parent.MoveAndSlide();
			
			// play jump/move animation. If animation is already playing, the animation is NOT started again from the beginning
			String animation_name = (Parent as Slime).GetMagicTypeAsString() + "_jump";
			Animations.Play(animation_name);
		}
		return null;
	}
}
