using Godot;
using System;

public partial class DungeonClearMenu : BaseMenu
{
	/**
	When instanciated, generate the augments, add them to the player and display them.
	*/
	public override void _Ready()
	{
		base._Ready();
		// get reference to augment display
		HBoxContainer augmentDisplay = GetNode<HBoxContainer>("%AugmentDisplay");
		// determine how many augments to drop
		Random rnd = new Random();

		uint amountAugments = (uint)rnd.Next(1, 3); // TODO once curses and everything is implemented, change this to something better

		for (int i = 0; i < amountAugments; i++)
		{
			GD.Print("new augment");
			uint amountAugmentEffects = (uint)rnd.Next(1, 3); // TODO determine this some other way
			Augment augment = AugmentManager.Instance.CreateRandomAugment(amountAugmentEffects);
			(GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player).AddAugmentToInventory(augment); // add the augment to the player
			// Display the augment
			InventoryItem displayItem = new InventoryItem{Augment = augment};
			PanelContainer container = new PanelContainer();
			container.CustomMinimumSize = new Vector2(100, 100);
			augmentDisplay.AddChild(container);
			container.AddChild(displayItem);
		}
	}

	/**
	Called when the ExitDungeonButton is pressed.
	CallDeferreds the exitdungeon function. Need to use call deferred to prevent bugs
	*/
	public void OnExitButtonPressed()
	{
		CallDeferred(nameof(ExitDungeon));
	}

	/**
	Returns the game to the main hub
	*/
	private void ExitDungeon()
	{
		SetRootMenu(MenuManager.MenuType.MainHub);
	}
}
