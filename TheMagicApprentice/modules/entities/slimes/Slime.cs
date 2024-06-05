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
	This needs be called whenever a new slime is added to a scene. 
	Set magic type, slime size and slime attack range of the slime. 
	Furthermore the position of the slime in the dungeon is set here. 
	If the slime size is large, the collision shapes of the slime are scaled up. 
	The default collision shapes fit the small slime.
	*/
	public void SetSlimeProperties(MagicType magicType, SlimeSize slimeSize, SlimeAttackRange slimeAttackRange, Vector2 slimePosition)
	{
		_magicType = magicType;
		_slimeSize = slimeSize;
		_slimeAttackRange = slimeAttackRange;
		Position = slimePosition;

		if (slimeSize == SlimeSize.LARGE) // Scale the collision shapes for the large slimes 
		{
			Vector2 scale = new Vector2((float)1.5, (float)1.5); // TODO: correct scale factor has to be determined after the sprite of the large slime has been made, this is just a dummy value
			GetNode<CollisionShape2D>("%CollisionShapeSlime").Scale = scale;
			GetNode<CollisionShape2D>("%HitBoxSlime").Scale = scale;
		}
	}

	/**
	Getter for magic type of a slime.
	Currently used for the animations/state machine, since they need to know which type of slime they are.
	*/
	public String GetMagicTypeAsString()
	{
		if (_magicType == MagicType.SUN)
		{
			return "sun";
		}
		else if (_magicType == MagicType.COSMIC)
		{
			return "cosmic";
		}
		else if (_magicType == MagicType.DARK)
		{
			return "dark";
		}
		GD.Print("Slime has no magic type!");
		return null;
	}

	/**
	Getter for slime size.
	Will be used for the animations/state machine, since they need to know which type of slime they are.
	*/
	public String GetSlimeSizeAsString()
	{
		if (_slimeSize == SlimeSize.LARGE)
		{
			return "large";
		}
		if (_slimeSize == SlimeSize.SMALL)
		{
			return "small";
		}
		GD.Print("Slime has no size!");
		return null;
	}

	/**
	Getter for slime attack range.
	will be used for the animations/state machine, since they need to know which type of slime they are.
	*/
	public String GetSlimeAttackRangeAsString()
	{
		if (_slimeAttackRange == SlimeAttackRange.MELEE)
		{
			return "melee";
		}
		if (_slimeAttackRange == SlimeAttackRange.RANGED)
		{
			return "ranged";
		}
		GD.Print("Slime has no attack range!");
		return null;
	}
}
