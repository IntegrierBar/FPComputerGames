using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public StateMachine StateMachine; ///< Reference to the state machine of the player charackter

	[Export]
	public AnimationPlayer AnimationPlayer; ///< Reference to the animation player of the player charackter


	/**
	Is called when the player charackter enters the scene tree.
	Checks if the references to the state machine and the animation player are valid and then sends them to the state machine so that all states get the references
	*/
	public override void _Ready()
	{
		System.Diagnostics.Debug.Assert(StateMachine is not null);
		System.Diagnostics.Debug.Assert(AnimationPlayer is not null);
		StateMachine.Init(this, AnimationPlayer);
	}

	/**
	Is called every frame
	We simply forward the call to the state machine
	*/
	public override void _Process(double delta)
	{
		//GD.Print(" Process called");
		StateMachine.ProcessFrame(delta);
	}

	/**
	Is called every physics update
	We simply forward the call to the state machine
	*/
	public override void _PhysicsProcess(double delta)
	{
		//GD.Print("Physics Process called");
		StateMachine.ProcessPhysics(delta);
	}

	/**
	Is called whenever any Input from the user is unhandled
	We simply forward the call to the state machine
	*/
	public override void _UnhandledInput(InputEvent @event)
	{
		//base._UnhandledInput(@event);
		StateMachine.ProcessInput(@event);
	}
}
