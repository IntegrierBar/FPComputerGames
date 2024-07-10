using Godot;
using System;
using System.Linq;

public partial class Player : CharacterBody2D
{
	[Export]
	public StateMachine StateMachine; ///< Reference to the state machine of the player charackter

	[Export]
	public AnimationPlayer AnimationPlayer; ///< Reference to the animation player of the player charackter

	private Augment[] _activeAugments = new Augment[5]; ///< Array of the 5 active augmentc


	/**
	Is called when the player charackter enters the scene tree.
	Checks if the references to the state machine and the animation player are valid and then sends them to the state machine so that all states get the references
	*/
	public override void _Ready()
	{
		System.Diagnostics.Debug.Assert(StateMachine is not null, "StateMachine in Player is null");
		System.Diagnostics.Debug.Assert(AnimationPlayer is not null, "AnimationPlayer in Player is null");
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
	Is called whenever any Input from the user is unhandled
	We simply forward the call to the state machine
	*/
	public override void _UnhandledInput(InputEvent @event)
	{
		StateMachine.ProcessInput(@event);
	}

	public void EquipAugmentInSlot(Augment augment, uint slot)
	{
		System.Diagnostics.Debug.Assert(slot < 5, "slot index is larger then 4");

		if (_activeAugments[slot] is not null)
		{
			_activeAugments[slot].UnEquip(GetTree());
		}
		_activeAugments[slot] = augment;

		ApplyAugmentEffects();
		GD.Print(_activeAugments);
	}

	public void UnEquipAugmentFromSlot(uint slot)
	{
		EquipAugmentInSlot(null, slot);
	}

	/**
	Reapplies and recalculates all augment effect
	*/
	private void ApplyAugmentEffects()
	{
		// first unequip all augments
		foreach (Augment augment in _activeAugments)
		{
			augment?.UnEquip(GetTree());
		}

		// then reset all spell damages
		foreach (InventorySpell inventorySpell in GetTree().GetNodesInGroup(Globals.InventorySpellGroup).OfType<InventorySpell>())
		{
			inventorySpell.ResetDamage();
		}

		// finally equip all augment
		foreach (Augment augment in _activeAugments)
		{
			augment?.Equip(GetTree());
		}
	}


	public void TestEquip()
	{
		Augment augment = new Augment();
		augment._augmentEffects[0] = new PercentDamageForOneSpell();

		EquipAugmentInSlot(augment, 0);
	}
}
