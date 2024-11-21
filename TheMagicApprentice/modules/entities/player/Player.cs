using Godot;
using System;
using System.Linq;


/**
The Player class is the root node of the player scene. It initalizes the Players state machine and then forwards Input, Process and PhysicsProcess to the state machine.
It also manages the active augments of the player.
The scene is set as an autoload so that every part of the game can reference it.
Process is only enabled if the main_game scene is set as active scene in the MenuManager
*/
public partial class Player : CharacterBody2D
{
	[Export]
	public StateMachine StateMachine; ///< Reference to the state machine of the player charackter

	[Export]
	public AnimationPlayer AnimationPlayer; ///< Reference to the animation player of the player charackter

	private Augment[] _activeAugments = new Augment[5]; ///< Array of the 5 active augments
	private InventorySpell[] _spellSlots = new InventorySpell[3]; ///< Array of the 3 spellslots. Contains the references to the currently selected InventorySpells


	/**
	Is called when the player charackter enters the scene tree.
	Checks if the references to the state machine and the animation player are valid and then sends them to the state machine so that all states get the references
	*/
	public override void _Ready()
	{
		System.Diagnostics.Debug.Assert(StateMachine is not null, "StateMachine in Player is null");
		System.Diagnostics.Debug.Assert(AnimationPlayer is not null, "AnimationPlayer in Player is null");
		StateMachine.Init(this, AnimationPlayer);

		// get the MenuManager and connect the MenuChanged signal to the OnMenuChanged function
		MenuManager menuManager = GetTree().GetFirstNodeInGroup("menu_manager") as MenuManager;
		if (menuManager is not null) // this is only false for tests and just exists for them
		{
			menuManager.MenuChanged += OnMenuChanged;
		}
		
	}

	/**
	Whenever the menu of the MenuManager changes, this function gets called.
	If the new menu is the main game scene, we activate ProcessMode and make the UI visible, otherwise we deactivate it and make the UI invisible.
	*/
	private void OnMenuChanged(MenuManager.MenuType newMenu, bool isPush)
	{
		if (newMenu == MenuManager.MenuType.MainGame)
		{
			ProcessMode = ProcessModeEnum.Inherit;
			//Visible = true;
			(GetNode("UI") as CanvasLayer).Visible = true;
		}
		else
		{
			ProcessMode = ProcessModeEnum.Disabled;
			//Visible = false;
			(GetNode("UI") as CanvasLayer).Visible = false;
		}
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

		// Reset the spell groups
		string[] spellGroupNames =  {Globals.Spell1, Globals.Spell2, Globals.Spell3};
		for (int i = 0; i < 3; i++)
		{
			// first remove all spells from the group
			var spellsInSpellGroup = GetTree().GetNodesInGroup(spellGroupNames[i]);
			foreach (var spell in spellsInSpellGroup)
			{
				spell.RemoveFromGroup(spellGroupNames[i]); 
			}
			// Then add the current spell if it exists
			_spellSlots[i]?.AddToGroup(spellGroupNames[i]);
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

	/**
	Sets skills for the player.
	First, all augments are removed, then the currently equipped spell is removed.
	If a new spell should be set, it is added to the group corresponding to the spell slot.
	Afterwards, the augments are added again. 
	Note: spell is null if a spell should be removed! This is intended behaviour.
	*/
	public void SetPlayerSkill(int nrSkillSlot, SpellName? spell)
	{
		UnEquipAllAugments();
		ResetSpells();
        
		// Remove old skill from group TODO should not be neccessary anymore
		String spellSlotGroupName = Globals.GetGroupNameOfSpellsInSlot((uint)nrSkillSlot); // TODO: Check that nothing can go wrong here
		var spellsInSpellGroup = GetTree().GetNodesInGroup(spellSlotGroupName);
		foreach (var oldSpell in spellsInSpellGroup)
		{
			oldSpell.RemoveFromGroup(spellSlotGroupName);
		}
		
        // Equip the spell in the slot (spell can also be null here)
		InventorySpell newSpell = null;
		if (spell is not null)
		{
			string spellGroupName = Globals.GetGroupNameOfSpell((SpellName)spell);
			newSpell = GetTree().GetFirstNodeInGroup(spellGroupName) as InventorySpell;
			newSpell.AddToGroup(spellSlotGroupName);
		}
		_spellSlots[nrSkillSlot] = newSpell;
		
		// Equip all augments again
		EquipAllAugments();
	}

	/**
	Get the reference to the InventorySpell at the index
	*/
	public InventorySpell GetPlayerSkill(int slotIndex)
	{
		return _spellSlots.ElementAtOrDefault(slotIndex);
	}

	// small test function to test augment generation
	public void TestEquip()
	{
		Augment augment = AugmentManager.Instance.CreateRandomAugment(3);

		EquipAugmentInSlot(augment, 0);

	}

	/**
	Show the AugmentInventory by setting its visibility to true.
	Ensuring that the inventory is shown by switching to it first.
	Is called by the main hub scirpt when the button to open the menu is pressed
	*/
	public void OpenAugmentInventory()
	{
		GetNode<AugmentInventory>("AugmentInventory").SwitchToAugmentInventory();
		GetNode<AugmentInventory>("AugmentInventory").SetVisibility(true);
	}

	/**
	Show the Fuse Augment Menu by setting the AugmentInventory visibility to true.
	Ensuring that the fusing menu is shown by switching to it first.
	Is called by the main hub scirpt when the button to open the menu is pressed
	*/
	public void OpenFuseAugments()
	{
		GetNode<AugmentInventory>("AugmentInventory").SwitchToFuseAugments();
		GetNode<AugmentInventory>("AugmentInventory").SetVisibility(true);
	}

	/**
	Show the Skill Tree by setting the SkillTree visibility to true.
	TODO: Is called by the main hub scirpt when the button to open the menu is pressed
	*/
	public void OpenSkillTree()
	{
		GetNode<SkillTree>("SkillTree").SetVisibility(true);
	}

	/**
	Adds the augment to the inventory
	*/
	public void AddAugmentToInventory(Augment augment)
	{
		GetNode<AugmentInventory>("AugmentInventory").AddAugmentToInventory(augment);
	}

	/**
	Add a skill point of type magicType.
	Is called by the dungeon clear menu and is used to forward to the skill tree.
	*/
	public void AddSkillPointOfType(MagicType magicType)
	{
		GetNode<SkillTree>("SkillTree").AddSkillPointOfType(magicType);
	}
}
