using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

public partial class PlayerDeathMenu : BaseMenu
{
	/**
	Gets called when the player presses the Exit Dungeon button.
	Simply calls ExitDungeon using CallDeferred to avoid problems
	*/
	public void OnExitDungeonButtonPressed()
	{
		CallDeferred(nameof(ExitDungeon));
	}

	/**
	Gets called when the player presses the Try Again button.
	Simply calls TryAgain using CallDeferred to avoid problems
	*/
	public void OnTryAgainButtonPressed()
	{
		CallDeferred(nameof(TryAgain));
	}

	/**
	Exit the dungeon by setting the root menu to mainHub
	*/
	private void ExitDungeon()
	{
		SetRootMenu(MenuManager.MenuType.MainHub);
	}

	/**
	Try again by resetting player hp and reloading the dungeon
	*/
	private void TryAgain()
	{
		(GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player).ResetPlayerHP();
		(GetTree().GetFirstNodeInGroup("dungeon_handler") as DungeonHandler).ReloadDungeon();

		// finally pop this menu
		PopMenu();
	}
}
