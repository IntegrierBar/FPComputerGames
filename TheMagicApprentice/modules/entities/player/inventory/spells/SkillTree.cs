using Godot;
using System;
using System.Security.Cryptography;

[GlobalClass]
public partial class SkillTree : CanvasLayer
{
	private Player player; ///< Reference to player to set skills 
	
	public override void _Ready()
	{
		player = GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player;

		int indexInitialSkill = 0;
		GetNode<OptionButton>("%OptionsSkillSlot1").Select(indexInitialSkill);
		SkillSlot1Selected(indexInitialSkill); 
		// TODO: the first skill should be selected in the intro dungeon depending on the chosen magic type
	}

	/**
	If Esc is pressed the SkillTree becomes invisible again and stops processing
	*/
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsAction("esc"))
		{
			SetVisibility(false);
		}
    }

	/**
	Set the visibility and the ProcessMode of the SkillTree. I.e. enable and disable it.
	*/
	public void SetVisibility(bool isVisible)
	{
		Visible = isVisible;
		if (isVisible)
		{
			ProcessMode = ProcessModeEnum.Always;
		}
		else
		{
			ProcessMode = ProcessModeEnum.Disabled;
		}
	}

	/**
	Gets called when the left button of the skill tree menu is pressed.
	Handles transition to Augment Inventory
	*/
	public void LeftButtonPressed()
	{
		SetVisibility(false);
		player.GetNode<AugmentInventory>("AugmentInventory").SwitchToAugmentInventory();
		player.GetNode<AugmentInventory>("AugmentInventory").SetVisibility(true);
	}

	/**
	Gets called when the right button of the skill tree menu is pressed.
	Handles transition to Fusing Augments Menu
	*/
	public void RightButtonPressed()
	{
		SetVisibility(false);
		player.GetNode<AugmentInventory>("AugmentInventory").SwitchToFuseAugments();
		player.GetNode<AugmentInventory>("AugmentInventory").SetVisibility(true);
	}

	/**
	Gets called when the skill in skill slot 1 gets changed. The skill that is equipped to skill slot 1
	is disabled for skill slot 2 and 3. Afterwards call function that equips the chosen skill to the player. 
	*/
	private void SkillSlot1Selected(int index)
	{
		for (int i = 0; i <= 8; i++) // enable all skills in skill slot 2 and 3 first to remove disabling from previous skill
		{
			GetNode<OptionButton>("%OptionsSkillSlot2").SetItemDisabled(i, false);
			GetNode<OptionButton>("%OptionsSkillSlot3").SetItemDisabled(i, false);
		}
		// Disable the currently chosen skill for slot 1 for the skill slots 2 and 3
		GetNode<OptionButton>("%OptionsSkillSlot2").SetItemDisabled(index, true); 
		GetNode<OptionButton>("%OptionsSkillSlot3").SetItemDisabled(index, true);
		SetSkill(0, index);
	}

	/**
	Gets called when the skill in skill slot 2 gets changed. To handle the empty option, a separate 
	function is called that adds and removes the empty option from the OptionButton. 
	Afterwards call function that equips the chosen skill to the player. 
	*/
	private void SkillSlot2Selected(int index)
	{
		OptionButton node = GetNode<OptionButton>("%OptionsSkillSlot2");
		HandleEmptyOption(node, index);
		
		SetSkill(1, index);
	}

	/**
	Gets called when the skill in skill slot 3 gets changed. To handle the empty option, a separate 
	function is called that adds and removes the empty option from the OptionButton. 
	Afterwards call function that equips the chosen skill to the player. 
	*/
	private void SkillSlot3Selected(int index)
	{
		OptionButton node = GetNode<OptionButton>("%OptionsSkillSlot3");
		HandleEmptyOption(node, index);

		SetSkill(2, index);
	}

	/**
	Adds or removes the empty option for the option buttons 2 and 3. Add empty button if a skill is 
	selected for the skill slots, remove the empty button and set the button to empty when empty is selected.
	The empty selection has the index 9.
	*/
	private void HandleEmptyOption(OptionButton node, int index)
	{
		int numberOfUnlockedSpells = 9;
		if (index == 9) 
		{
			SetEmptyOption(node);
		}
		else if (node.ItemCount == numberOfUnlockedSpells)
		{
			node.AddItem("Empty", 9);
		}
	}

	/**
	Sets empty selection for the OptionButton and removes the Empty Option
	*/
	private void SetEmptyOption(OptionButton node)
	{
		node.Select(-1);
		node.RemoveItem(9);
	}

	/**
	This function handles the calls to the corresponding functions in the player class that equip the spells 
	to the PC. 
	If a skill is already equipped to another skill slot, it is removed from that skill slot automatically 
	and equipped to the new skill slot. (This cannot happen with skill slot 1.)
	*/
	private void SetSkill(int nrSkillSlot, int index)
	{
		int indexOfDublicateSkill = IsSkillCurrentlyEquipped(nrSkillSlot, index);
		if (indexOfDublicateSkill == 1 || indexOfDublicateSkill == 2) 
		{
			player.SetPlayerSkills(indexOfDublicateSkill, null);
			String nodeName = "%OptionsSkillSlot" + (indexOfDublicateSkill + 1);
			OptionButton node = GetNode<OptionButton>(nodeName);
			SetEmptyOption(node);
			SetUIIcon(indexOfDublicateSkill, -1);
		}
		#nullable enable
			SpellName? spell = GetSpellFromIndex(index);
			player.SetPlayerSkills(nrSkillSlot, spell);
		#nullable disable
		SetUIIcon(nrSkillSlot + 1, index);
	}

	/**
	This function calls the SpellInventory to change the icons of the spells 
	*/
	private void SetUIIcon(int nrSpellSlot, int index)
	{
		String skillNodeName = "%OptionsSkillSlot" + (nrSpellSlot);
		OptionButton skillSlotNode = GetNode<OptionButton>(skillNodeName);
		#nullable enable
			Texture2D? spellIcon = skillSlotNode.GetItemIcon(index);
			player.GetNode<SpellInventory>("UI/SpellInventory").SetUIIcon(nrSpellSlot, spellIcon);
		#nullable disable
	}

	/**
	Function that checks if the selected skill is already equipped to a different skill slot
	Note: This function can only find one additional copy of the skill, not several. But there should 
	never be more than one other copy of any spell.
	Caution: nrSkillSlot ranges from 0 to 2, while the OptionButtons have numbers from 1 to 3
	*/
	private int IsSkillCurrentlyEquipped(int nrSkillSlot, int index)
	{
		OptionButton node1 = GetNode<OptionButton>("%OptionsSkillSlot2");
		OptionButton node2 = GetNode<OptionButton>("%OptionsSkillSlot3");

		if (nrSkillSlot != 1)
		{
			if (node1.GetSelectedId() == index)
			{
				return 1;
			}
		}
		if (nrSkillSlot != 2)
		{
			if (node2.GetSelectedId() == index)
			{
				return 2;
			}
		}
		// no duplicate skill was found
		return -1;
	}

	private void UnlockButtonPressed()
	{
		
		// Handle unlocking of skills here
		// check whether that is possible in the current state and with the current amount of skill points first
		
	}

	/**
	Function that gets the SpellName corresponding to each index from the OptionsButton
	Function returns null if index is not between 0 and 8!
	*/
	private static SpellName? GetSpellFromIndex(int index) => index switch
	{
		0 => SpellName.SunBasic,
		1 => SpellName.SunBeam,
		2 => SpellName.SummonSun,
		3 => SpellName.CosmicBasic,
		4 => SpellName.MoonLight,
		5 => SpellName.StarRain,
		6 => SpellName.DarkBasic,
		7 => SpellName.DarkEnergyWave,
		8 => SpellName.BlackHole,
		_ => null,
	};

}
