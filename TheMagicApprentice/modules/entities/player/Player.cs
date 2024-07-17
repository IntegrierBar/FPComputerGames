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

	/**
	Equips an Augment at slot
	*/
	public void EquipAugmentInSlot(Augment augment, int slot)
	{
		System.Diagnostics.Debug.Assert(slot < 5, "slot index is larger then 4");

		slot = Math.Clamp(slot, 0, 4); // clamp slot


		UnEquipAllAugments();
		ResetSpells();
		_activeAugments[slot] = augment;
		EquipAllAugments();
	}

	/**
	Remove an augment from a slot by equipping null in the slot
	*/
	public void UnEquipAugmentFromSlot(int slot)
	{
		EquipAugmentInSlot(null, slot);
	}

	/**
	Calls Equip on all Augments
	*/
	private void EquipAllAugments()
	{
		foreach (Augment augment in _activeAugments)
		{
			augment?.Equip(GetTree());
		}
	}

	/**
	Calls UnEquip on all Augments
	*/
	private void UnEquipAllAugments()
	{
		foreach (Augment augment in _activeAugments)
		{
			augment?.UnEquip(GetTree());
		}
	}

	/**
	Resets the damage of all spells and recalculates which spells are cast
	*/
	private void ResetSpells()
	{
		// then reset all spell damages and remove the OnCastAugments
		foreach (InventorySpell inventorySpell in GetTree().GetNodesInGroup(Globals.InventorySpellGroup).OfType<InventorySpell>())
		{
			inventorySpell.ResetDamage();
			inventorySpell.ClearOnCastAugmentEffects();
		}

		// This is a little bit ugly, but it works
		// loop over all spell groups, keep the first spell and remove all others
        foreach (var spellGroup in new string[3] {Globals.Spell1, Globals.Spell2, Globals.Spell3})
        {
            var spellsInSpellGroup = GetTree().GetNodesInGroup(spellGroup);
			for (int i = 1; i < spellsInSpellGroup.Count; i++) // start index at 1 since we want to keep the first node
			{
				spellsInSpellGroup[i].RemoveFromGroup(spellGroup); // remove from the spell Group
			}
        }
	}

	/**
	Recalculates all augment effects
	*/
	public void RecalculateAugmentEffects()
	{
		UnEquipAllAugments();
		ResetSpells();
		EquipAllAugments();
	}

	// small test function to test augment generation
	public void TestEquip()
	{
		Augment augment = AugmentManager.Instance.CreateRandomAugment(3);

		EquipAugmentInSlot(augment, 0);
	}
}
