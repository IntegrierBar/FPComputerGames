using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public StateMachine StateMachine;

	[Export]
	public AnimationPlayer AnimationPlayer;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		System.Diagnostics.Debug.Assert(StateMachine is not null);
		StateMachine.Init(this, AnimationPlayer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		StateMachine.ProcessFrame(delta);
	}
}
