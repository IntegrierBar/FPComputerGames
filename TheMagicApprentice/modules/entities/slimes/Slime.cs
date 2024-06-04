using Godot;
using System;

public partial class Slime : CharacterBody2D
{
	[Export]
	public StateMachine StateMachine; ///< Reference to the state machine of the slime

	[Export]
	public AnimationPlayer AnimationPlayer; ///< Reference to the animation player of the slime

	private MagicType _magicType; ///< Defines the slimes magic type

	private SlimeSize _slimeSize; ///< Defines the slimes size

	private SlimeAttackRange _slimeAttackRange; ///< Defines whether the slime is melee or ranged

	/**
	Is called when the slime enters the scene tree.
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
		StateMachine.ProcessFrame(delta);
	}

	/**
	Is called every physics update
	We simply forward the call to the state machine
	*/
	public override void _PhysicsProcess(double delta)
	{
		StateMachine.ProcessPhysics(delta);
	}

	/**
	Allows to set the properties of the slime. 
	This should be called whenever a new slime is added to a scene. 
	*/
	public void SetSlimeProperties(MagicType magicType, SlimeSize slimeSize, SlimeAttackRange slimeAttackRange)
	{
		_magicType = magicType;
		_slimeSize = slimeSize;
		_slimeAttackRange = slimeAttackRange;
	}
}
