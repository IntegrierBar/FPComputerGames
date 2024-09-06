using Godot;
using System;

public partial class SkillTree : CanvasLayer
{
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
		GetNode<AugmentInventory>("AugmentInventory").SwitchToAugmentInventory();
		GetNode<AugmentInventory>("AugmentInventory").SetVisibility(true);
	}

	/**
	Gets called when the right button of the skill tree menu is pressed.
	Handles transition to Fusing Augments Menu
	*/
	public void RightButtonPressed()
	{
		SetVisibility(false);
		GetNode<AugmentInventory>("AugmentInventory").SwitchToFuseAugments();
		GetNode<AugmentInventory>("AugmentInventory").SetVisibility(true);
	}

}
