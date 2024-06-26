using Godot;
using System;

public partial class Unicorn : CharacterBody2D
{
	[Export]
	public StateMachine StateMachine; ///< Reference to the state machine of the unicorn
	[Export]
	public AnimationPlayer AnimationPlayer; ///< Reference to the animation player of the unicorn

	[Export]
	public float MeleeAttackRange = 10; ///< If the PC is inside of this radius, the unicorn performs a melee attack

	private MagicType _magicType; ///< Magic type of the unicorn

	/**
	Is called when the unicorn enters the scene tree.
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

	public void SetUnicornProperties(MagicType magicType, Vector2 unicornPosition)
	{
		_magicType = magicType;
		SetArmorValues(magicType);
		Position = unicornPosition;
	}

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

	public MagicType GetMagicType()
	{
		return _magicType;
	}
}
