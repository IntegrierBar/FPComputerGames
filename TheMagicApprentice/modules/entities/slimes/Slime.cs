using Godot;
using System;

public partial class Slime : CharacterBody2D
{
	[Export]
	public StateMachine StateMachine; ///< Reference to the state machine of the slime
	[Export]
	public AnimationPlayer AnimationPlayer; ///< Reference to the animation player of the slime

	[Export]
	public float ViewRange = 100; ///< Range in which slimes can detect the PC (currently the same for melee and ranged, but this can be changed)
	[Export]
	public float AttackRangeMelee = 10; ///< Range from which a melee slime can attack the PC
	[Export]
	public float AttackRangeRanged = 50; ///< Range from which a ranged slime can attack the PC

	[Export]
	public float BaseDamage = 10;

	private MagicType _magicType; ///< Defines the slimes magic type
	private SlimeSize _slimeSize; ///< Defines the slimes size
	private SlimeAttackRange _slimeAttackRange; ///< Defines whether the slime is melee or ranged

	private float _viewRange; ///< View range of this slime
	private float _attackRangeF; ///< Attack range of this slime
	private float _damageValue; ///< Damage the slime applies when attacking

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
	Also set view range and attack range for the slime.
	*/
	public void SetSlimeProperties(MagicType magicType, SlimeSize slimeSize, SlimeAttackRange slimeAttackRange, Vector2 slimePosition)
	{
		_damageValue = BaseDamage;

		_magicType = magicType;
		_slimeSize = slimeSize;
		_slimeAttackRange = slimeAttackRange;
		Position = slimePosition;
		_viewRange = ViewRange; // Note: if view range should be different for melee and ranged slimes later, this has to go into the if-else part 

		if (slimeSize == SlimeSize.LARGE) // Scale the collision shapes for the large slimes 
		{
			Vector2 scale = new Vector2((float)1.5, (float)1.5); // TODO: correct scale factor has to be determined after the sprite of the large slime has been made, this is just a dummy value
			GetNode<CollisionShape2D>("%CollisionShapeSlime").Scale = scale;
			GetNode<CollisionShape2D>("%HitBoxSlime").Scale = scale;
		}

		if (slimeAttackRange == SlimeAttackRange.MELEE) // set attack range of the slime depending on their attack range type
		{
			_attackRangeF = AttackRangeMelee;
		}
		else
		{
			_attackRangeF = AttackRangeRanged;
		}
	}

	/**
	Getter for magic type of a slime.
	*/
	public MagicType GetMagicType()
	{
		return _magicType;
	}

	/**
	Getter for magic type of a slime, converted to a string.
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
	Getter for slime size, converted to a string.
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
	Getter for slime attack range, converted to a string.
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

	/**
	Getter for view range of the slime.
	*/
	public float GetViewRange()
	{
		return _viewRange;
	}

	/**
	Getter for attack range of the slime.
	Using this function ensures that the correct attack range is returned for every slime independent of their attack range type.
	*/
	public float GetAttackRangeF()
	{
		return _attackRangeF;
	}

	public float GetDamageValue()
	{
		return _damageValue;
	}
}
