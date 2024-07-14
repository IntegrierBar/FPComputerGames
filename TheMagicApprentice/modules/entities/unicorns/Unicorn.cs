using Godot;
using System;

public partial class Unicorn : CharacterBody2D
{
	[Export]
	public StateMachine StateMachine; ///< Reference to the state machine of the unicorn
	[Export]
	public AnimationPlayer AnimationPlayer; ///< Reference to the animation player of the unicorn

	[Export]
	public float MeleeAttackRange = 50; ///< If the PC is inside of this radius, the unicorn performs a melee attack

	[Export]
	public float BaseDamage = 25; ///< Basic damage of the unicorn

	private MagicType _magicType; ///< Magic type of the unicorn
	private float _damageValue; ///< Actual damage of the unicorn

	/**
	Is called when the unicorn enters the scene tree.
	Checks if the references to the state machine and the animation player are valid and then sends them to the state machine so that all states get the references
	*/
	public override void _Ready()
	{
		System.Diagnostics.Debug.Assert(StateMachine is not null, "StateMachine in Unicorn is null");
		System.Diagnostics.Debug.Assert(AnimationPlayer is not null, "AnimationPlayer in Unicorn is null");
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
	Sets properties of the unicorn: Magic type, initial position and base damage.
	Modifications of the base damage could be done here. 
	The armor values of the unicorn are set depending on the magic type of the unicorn. 
	*/
	public void SetUnicornProperties(MagicType magicType, Vector2 unicornPosition)
	{
		_damageValue = BaseDamage;
		_magicType = magicType;
		SetArmorValues(magicType);
		Position = unicornPosition;
	}

	/**
	Set armor values of the unicorn depending on the magic type of the unicorn. 
	The unicorn has an armor of 50 against the magic type it is strong against, an armor value of 30 
	against its own magic type and an armor value of 5 against the magic type it is weak against. 
	*/
	private void SetArmorValues(MagicType magicType)
	{
		double armorSun;
		double armorCosmic;
		double armorDark;
		if (magicType == MagicType.SUN)
		{
			armorSun = 30;
			armorCosmic = 50;
			armorDark = 5;
			GetNode<HealthComponent>("%HealthComponent").SetArmor(armorSun, armorCosmic, armorDark);
		}
		if (magicType == MagicType.COSMIC)
		{
			armorSun = 5;
			armorCosmic = 30;
			armorDark = 50;
			GetNode<HealthComponent>("%HealthComponent").SetArmor(armorSun, armorCosmic, armorDark);
		}
		else // magic type dark
		{
			armorSun = 50;
			armorCosmic = 5;
			armorDark = 30;
			GetNode<HealthComponent>("%HealthComponent").SetArmor(armorSun, armorCosmic, armorDark);
		}
	}

	/**
	Getter for magic type of the unicorn.
	*/
	public MagicType GetMagicType()
	{
		return _magicType;
	}

	/**
	Getter for damage value of the unicorn.
	*/
	public float GetDamageValue()
	{
		return _damageValue;
	}
}
