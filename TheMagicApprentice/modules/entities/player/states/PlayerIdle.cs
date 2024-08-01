using Godot;
using System;

public partial class PlayerIdle : State
{
	[ExportGroup("States")]
	//[Export]
	//public State Idle;
	[Export]
	public State Moving; ///< Reference to Moving state
	[Export]
	public State Dashing; ///< Reference to Dashing state 
	[Export]
	public State SpellCasting; ///< Reference to SpellCasting state

	/**
	Play idle animation when we enter this state
	*/
	public override void Enter()
	{
		base.Enter();
		Animations.Play("idle");
	}


	public override State ProcessPhysics(double delta)
	{
		// we out these checks into then Physics process function in order to take care of the case 
		// that we exit spellcasting or dashing state while pressing another movement button.
		if (Input.GetVector("left", "right", "up", "down") != Vector2.Zero)
		{
			return Moving;
		}
		if (Input.IsActionPressed("dash"))
		{
			return Dashing;
		}
		if (@Input.IsActionPressed("cast"))
		{
			return SpellCasting;
		}
		return null;
	}
}
