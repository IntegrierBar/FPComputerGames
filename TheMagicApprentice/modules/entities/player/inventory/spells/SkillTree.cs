using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

[GlobalClass]
public partial class SkillTree : CanvasLayer
{
	private Player player; ///< Reference to player to set skills 

	private ButtonGroup _skillTreeButtons; ///< reference to the ButtonGroup the the buttons inside the skillTree
	private int _indexOfSkill1; ///< Index of the skill selected in Skill slot 1.
	
	public override void _Ready()
	{
		player = GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player;

		int indexInitialSkill = 0;
		GetNode<OptionButton>("%OptionsSkillSlot1").Select(indexInitialSkill);
		SkillSlot1Selected(indexInitialSkill); 
		// TODO: the first skill should be selected in the intro dungeon depending on the chosen magic type

		// get the reference to the skill tree button group (this seems to be the cannonical way of doing this)
		_skillTreeButtons = GetNode<TextureButton>("MarginContainer/MarginContainer2/VBoxContainer/VBoxContainer/HBoxContainer/MarginContainerSun/VBoxContainer/SunBasicPanelContainer/SunBasic").ButtonGroup;

		AddSkillPointOfType(MagicType.SUN);
		AddSkillPointOfType(MagicType.SUN);
		AddSkillPointOfType(MagicType.SUN);
		AddSkillPointOfType(MagicType.SUN);
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
	Unlocks the basic skill of the magicType and sets it as the skill in slot 1.
	Is used whenever a new game is started to set the first spell of the game
	*/
	public void SetStartBasic(MagicType magicType)
	{
		SpellName basicSpell = magicType switch // get the SpellName of the Basic Spell
		{
			MagicType.SUN => SpellName.SunBasic,
			MagicType.COSMIC => SpellName.CosmicBasic,
			MagicType.DARK => SpellName.DarkBasic,
			_ => SpellName.SunBasic, // default value that realistically should not happen, but it makes static analyzer happy.
		};

		var skillTreeButtons = _skillTreeButtons.GetButtons();
		// loop over all buttons, find the correct button and disable it, to show that the skill is unlocked
		foreach (var button in skillTreeButtons)
		{
			if (ConvertStringNameToSpellName(button.Name.ToString()) == basicSpell)
			{
				button.Disabled = true;
				break;
			}
		}
		// Unlock the skill and set it in slot 1
		UnlockSkillInSelectionMenu(basicSpell);
		SkillSlot1Selected(GetIndexFromSpell(basicSpell));
	}

	/**
	Gets called when the skill in skill slot 1 gets changed. The skill that is equipped to skill slot 1
	is disabled for skill slot 2 and 3. Afterwards call function that equips the chosen skill to the player. 
	*/
	private void SkillSlot1Selected(int index)
	{
		
		GetNode<OptionButton>("%OptionsSkillSlot2").SetItemDisabled(_indexOfSkill1, false);
		GetNode<OptionButton>("%OptionsSkillSlot3").SetItemDisabled(_indexOfSkill1, false);
		
		// Disable the currently chosen skill for slot 1 for the skill slots 2 and 3
		GetNode<OptionButton>("%OptionsSkillSlot2").SetItemDisabled(index, true); 
		GetNode<OptionButton>("%OptionsSkillSlot3").SetItemDisabled(index, true);
		SetSkill(0, index);
		_indexOfSkill1 = index;
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
			player.SetPlayerSkill(indexOfDublicateSkill, null);
			String nodeName = "%OptionsSkillSlot" + (indexOfDublicateSkill + 1);
			OptionButton node = GetNode<OptionButton>(nodeName);
			SetEmptyOption(node);
			SetUIIcon(indexOfDublicateSkill, -1);
		}
		#nullable enable
			SpellName? spell = GetSpellFromIndex(index);
			player.SetPlayerSkill(nrSkillSlot, spell);
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
	public int IsSkillCurrentlyEquipped(int nrSkillSlot, int index)
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

	/**
	Gets called when the Unlock Button was pressed.
	Finds out which spell the player has selected to be unlocked.
	If the player has enough skill points of that type, the skill is unlocked and the button for the 
	*/
	private void UnlockButtonPressed()
	{
		// get the currently selected skill and check if we have enough skill points of its type
		BaseButton currentButton = _skillTreeButtons.GetPressedButton();
		// if there is no button selected or the currently selected button is disabled (i.e. is already unlocked) do nothing.
		if (currentButton is null || currentButton.Disabled)
		{
			return;
		}

		// Get the magic Type of the skill from its string name, so that we can determine the skill points
		SpellName spellName = ConvertStringNameToSpellName(currentButton.Name.ToString());
		MagicType magicTypeOfSkill = EntityTypeHelper.GetMagicTypeOfSpell(spellName);

		int skillPoints = GetSkillPointsOfType(magicTypeOfSkill);
		if (skillPoints <= 0 || !CanUnlockSkill(spellName)) // if we dont have enough skill points, or the basic skill of the magic type is locked do nothing
		{
			return;
		}

		// unlock the skill and reduce the skill points by one
		UnlockSkillInSelectionMenu(spellName);
		SetSkillPointsOfType(magicTypeOfSkill, skillPoints - 1);
		// finally disable the button so that it cannot be used again
		currentButton.Disabled = true;
		currentButton.ButtonPressed = false;
	}

	/**
	Function that gets the SpellName corresponding to each index from the OptionsButton
	Function returns null if index is not between 0 and 8!
	*/
	public SpellName? GetSpellFromIndex(int index) => index switch
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

	/**
	Returns the index of the SpellName from the OptionsButton.
	Is the inverse of GetSpellFromIndex.
	Is not static since technically it depends on implementation that might in the future depend on the scene.
	*/
	public int GetIndexFromSpell(SpellName spellName) => spellName switch
	{
		SpellName.SunBasic => 0,
		SpellName.SunBeam => 1,
		SpellName.SummonSun => 2,
		SpellName.CosmicBasic => 3,
		SpellName.MoonLight => 4,
		SpellName.StarRain => 5,
		SpellName.DarkBasic => 6,
		SpellName.DarkEnergyWave => 7,
		SpellName.BlackHole => 8,
		_ => 0,
	};

	/**
	Converts the StringName of a spell into the actual SpellName. 
	Is used to convert the Name of the buttons that are the skill tree into the corresponding SpellName
	*/
	public static SpellName ConvertStringNameToSpellName(string skillName) => skillName switch
	{
		"SunBasic" => SpellName.SunBasic,
		"SummonSun" => SpellName.SummonSun,
		"SunBeam" => SpellName.SunBeam,
		"CosmicBasic" => SpellName.CosmicBasic,
		"MoonLight" => SpellName.MoonLight,
		"StarRain" => SpellName.StarRain,
		"DarkBasic" => SpellName.DarkBasic,
		"DarkEnergyWave" => SpellName.DarkEnergyWave,
		"BlackHole" => SpellName.BlackHole,
		_ => SpellName.SunBasic,
	};

	/**
	Returns the number of SkillPoints currently available of the MagicType.
	Does so by reading the value from the display string.
	Incase there is an error with converting the string to int, it resets the skill points to 0 and returns -1 to indicate that there was something wrong (this is only really used for the tests)
	*/
	public int GetSkillPointsOfType(MagicType magicType)
	{
		Label skillPointLabel = GetReferenceToSkillPointLabel(magicType); // get reference to label

		int skillPoints;
		if (! int.TryParse(skillPointLabel.Text, out skillPoints)) // convert string to int and check if it failed
		{
			// Could not convert skill points, so we reset them to 0
			skillPointLabel.Text = 0.ToString();
			skillPoints = -1;
		}
		return skillPoints;
	}

	/**
	Set the skill points of type magicType to skillPoints by setting the text.
	*/
	private void SetSkillPointsOfType(MagicType magicType, int skillPoints)
	{
		Label skillPointLabel = GetReferenceToSkillPointLabel(magicType); // get reference to label
		skillPointLabel.Text = skillPoints.ToString(); // set the label
	}

	/**
	Get Reference to the SkillPointLabel of the MagicType.
	*/
	private Label GetReferenceToSkillPointLabel(MagicType magicType)
	{
		switch (magicType)
		{
			case MagicType.SUN:
				return GetNode<Label>("%SunSkillPoints");
			case MagicType.COSMIC:
				return GetNode<Label>("%CosmicSkillPoints");
			case MagicType.DARK:
				return GetNode<Label>("%DarkSkillPoints");
			default: // cannot happen
				return null;
		}
	}

	/**
	Unlocks the skill in the skill selection menu
	*/
	private void UnlockSkillInSelectionMenu(SpellName spellName)
	{
        List<string> namesOfOptionButtons = new List<string>{"%OptionsSkillSlot1", "%OptionsSkillSlot2", "%OptionsSkillSlot3"}; // list of the names of the OptionButtons
		foreach (var namesOfOptionButton in namesOfOptionButtons)
		{
			GetNode<OptionButton>(namesOfOptionButton).SetItemDisabled(GetIndexFromSpell(spellName), false); // enable the item
		}
	}

	/**
	Add a skill point of the MagicType magicType
	*/
	public void AddSkillPointOfType(MagicType magicType)
	{
		Label skillPointLabel = GetReferenceToSkillPointLabel(magicType);
		int skillPoints = 0;
		if (int.TryParse(skillPointLabel.Text, out skillPoints))
		{
			skillPointLabel.Text = (skillPoints + 1).ToString();
		}
		else
		{
			// Could not convert skill points, so we reset them to 1, since we just added a skill point
			skillPointLabel.Text = 1.ToString();
		}
	}

	/**
	Checks if the skill can be unlocked.
	I.e. if it is not a base spell, the base spell of that magicType has to be unlocked
	*/
	private bool CanUnlockSkill(SpellName spellName)
	{
		var baseSpells = new HashSet<SpellName> {SpellName.SunBasic, SpellName.CosmicBasic, SpellName.DarkBasic};
		if (!baseSpells.Contains(spellName)) // if it is not a base spell, we have to check if its base spell is unlocked by checking if its base spell is unlocked in slot 1
		{
			MagicType magicTypeOfSkill = EntityTypeHelper.GetMagicTypeOfSpell(spellName);
			return !GetNode<OptionButton>("%OptionsSkillSlot1").IsItemDisabled((int)magicTypeOfSkill * 3); // the index of the basic spell of each MagicType is a multiple of 3 in the correct order;
		}
		return true;
	}
}
