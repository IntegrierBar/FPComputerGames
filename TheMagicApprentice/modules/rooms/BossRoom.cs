using Godot;
using System;

public partial class BossRoom : Node2D
{
	/**
	gets called when unicorn dies.
	pauses the game and shows a popup telling the player what augments they got together with the exit button
	*/
	public void OnUnicornDeath()
	{
		// TODO: for now create augment and exit the room
		uint amountAugmentEffects = 2; // TODO determine this some other way
		Augment augment = AugmentManager.Instance.CreateRandomAugment(amountAugmentEffects);
		(GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player).AddAugmentToInventory(augment); // add the augment to the player

		// exit the dungeon
		MenuManager menuManager = GetTree().GetFirstNodeInGroup("menu_manager") as MenuManager;
		menuManager.SetRootMenu(MenuManager.MenuType.MainHub);
	}
}
