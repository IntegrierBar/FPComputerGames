using Godot;
using System;

public partial class BossRoom : Node2D
{
	/**
	gets called when unicorn dies.
	*/
	public void OnUnicornDeath()
	{
		// use call deferred to prevent bugs
		CallDeferred(nameof(OpenMenu));
	}


	/**
	pauses the game and shows a popup telling the player what augments they got together with the exit button
	*/
	private void OpenMenu()
	{
		MenuManager menuManager = GetTree().GetFirstNodeInGroup("menu_manager") as MenuManager;
		menuManager.PushMenu(MenuManager.MenuType.DungeonClearMenu);
	}
}
