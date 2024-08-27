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
	public float HealthSmall = 100; ///< Health of the small slime
	[Export]
	public float HealthLarge = 200; ///< Health of the large slime

	[Export]
	public float BaseDamage = 10;

	private MagicType _magicType; ///< Defines the slimes magic type
	private SlimeSize _slimeSize; ///< Defines the slimes size
	private SlimeAttackRange _slimeAttackRange; ///< Defines whether the slime is melee or ranged

	private float _viewRange; ///< View range of this slime
	private float _attackRangeValue; ///< Attack range of this slime
	private float _damageValue; ///< Damage the slime applies when attacking

	/**
	Is called when the slime enters the scene tree.
	Checks if the references to the state machine and the animation player are valid and then sends them to the state machine so that all states get the references
	*/
	public override void _Ready()
	{
		System.Diagnostics.Debug.Assert(StateMachine is not null, "StateMachine in Slime is null");
		System.Diagnostics.Debug.Assert(AnimationPlayer is not null, "AnimationPlayer in Slime is null");
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
		SetArmorValues(magicType);
		_slimeSize = slimeSize;
		_slimeAttackRange = slimeAttackRange;
		Position = slimePosition;
		_viewRange = ViewRange; // Note: if view range should be different for melee and ranged slimes later, this has to go into the if-else part 

		double health;
		if (slimeSize == SlimeSize.LARGE)
		{
			health = HealthLarge;
		}
		else
		{
			health = HealthSmall;
		}

		if (CurseHandler.IsActive(Curse.MONSTER_BUFF))
		{
			GetNode<HealthComponent>("%HealthComponent").SetMaxHP(health * 1.3);
		}
		else
		{
			GetNode<HealthComponent>("%HealthComponent").SetMaxHP(health);
		}

		if (slimeSize == SlimeSize.LARGE) // Scale the collision shapes for the large slimes 
		{
			Vector2 scale = new Vector2((float)1.5, (float)1.5); // TODO: correct scale factor has to be determined after the sprite of the large slime has been made, this is just a dummy value
			GetNode<CollisionShape2D>("%CollisionShapeSlime").Scale = scale;
			GetNode<CollisionShape2D>("%HitBoxSlime").Scale = scale;
		}

		if (slimeAttackRange == SlimeAttackRange.MELEE) // set attack range of the slime depending on their attack range type
		{
			_attackRangeValue = AttackRangeMelee;
		}
		else
		{
			_attackRangeValue = AttackRangeRanged;
		}
	}

	/**
	Set armor values of the slime depending on the magic type of the slime. 
	The slime has an armor of 40 against the magic type it is strong against, an armor value of 25 
	against its own magic type and an armor value of 10 against the magic type it is weak against. 
	*/
	private void SetArmorValues(MagicType magicType)
	{
		double armorSun;
		double armorCosmic;
		double armorDark;
		switch (magicType)
		{
			case MagicType.SUN: 
				armorSun = 25;
				armorCosmic = 50;
				armorDark = 10;
				GetNode<HealthComponent>("%HealthComponent").SetArmor(armorSun, armorCosmic, armorDark);
				break;
			
			case MagicType.COSMIC:
				armorSun = 10;
				armorCosmic = 25;
				armorDark = 50;
				GetNode<HealthComponent>("%HealthComponent").SetArmor(armorSun, armorCosmic, armorDark);
				break;
			
			case MagicType.DARK:
				armorSun = 50;
				armorCosmic = 10;
				armorDark = 25;
				GetNode<HealthComponent>("%HealthComponent").SetArmor(armorSun, armorCosmic, armorDark);
				break;
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
	Getter for attack range type of a slime.
	*/
	public SlimeAttackRange GetSlimeAttackRange()
	{
		return _slimeAttackRange;
	}

	/**
	Getter for size of a slime.
	*/
	public SlimeSize GetSlimeSize()
	{
		return _slimeSize;
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
	public float GetAttackRangeValue()
	{
		return _attackRangeValue;
	}

	/**
	Getter for damage value of the slime.
	*/

	public float GetDamageValue()
	{
		float damageMultiplier = 1.0f;
		if (CurseHandler.IsActive(Curse.MONSTER_BUFF))
		{
			damageMultiplier = 1.1f;
		}
		return _damageValue * damageMultiplier;
	}
}
