using Godot;
using System;
using System.Collections.Generic;

public partial class NewGameMenu : BaseMenu
{
	private MagicType _magicTypeSelected; ///< Which element was selected by the player. Is actually just a weird way of using it as Input for EnterIntroDungeon(). Secause we need to use CallDeferred, we cannot use MagicType as Input because its not Godot.Variant.

	/**
	Resets the player scene
	*/
	public override void _Ready()
	{
		base._Ready();
		// Need to reset the player scene.
		//(GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player).ResetPlayer();
		//GetTree().CurrentScene = GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player;
		//GetTree().ReloadCurrentScene();
	}

	/**
	If the ESC key is pressed, return to main menu
	*/
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsAction("esc"))
		{
			PopMenu();
		}
	}

	/**
	Gets called when the button for the sun element is pressed
	*/
	public void SunSelected()
	{
		_magicTypeSelected = MagicType.SUN;
		SelectElementInPlayer(MagicType.SUN);
		CallDeferred(nameof(EnterIntroDungeon));
	}

	/**
	Gets called when the button for the cosmic element is pressed
	*/
	public void CosmicSelected()
	{
		_magicTypeSelected = MagicType.COSMIC;
		SelectElementInPlayer(MagicType.COSMIC);
		CallDeferred(nameof(EnterIntroDungeon));
	}

	/**
	Gets called when the button for the dark element is pressed
	*/
	public void DarkSelected()
	{
		_magicTypeSelected = MagicType.DARK;
		SelectElementInPlayer(MagicType.DARK);
		CallDeferred(nameof(EnterIntroDungeon));
	}

	/**
	Unlocks and selects the base spell of MagicType magicType in the player
	*/
	private void SelectElementInPlayer(MagicType magicType)
	{
		var player = GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player;
		player.SetStartBasic(magicType);
		
		// Set the starting magic type in ProgressManager
		var progressManager = GetTree().GetFirstNodeInGroup(Globals.ProgressManagerGroup) as ProgressManager;
		progressManager.SetPlayerStartMagicType(magicType);
	}

	/**
	Exits this scene and loads into the IntroDungeon.
	*/
	private void EnterIntroDungeon()
	{
		// get the intro dungeon by loading all dungeons from the dungeon file, then iteration over all of them to find the intro dungeon.
		string jsonPath = "res://game_configuration/story_dungeons.json";
		List<Dungeon> dungeons = DungeonHelper.LoadDungeonsFromFile(jsonPath);
		Dungeon introDungeon = null;
		foreach (var dungeon in dungeons)
		{
			if (dungeon.Name == "Intro")
			{
				introDungeon = dungeon;
				break;
			}
		}
		System.Diagnostics.Debug.Assert(introDungeon is not null, "Could not load intro dungeon from file");

		// change the MagicType of the IntroDungeon to the one selected by the player and load it
		introDungeon.MagicType = EntityTypeHelper.GetWeakerMagicType(_magicTypeSelected);
		(GetTree().GetFirstNodeInGroup("dungeon_handler") as DungeonHandler).SetDungeon(introDungeon);
		SetRootMenu(MenuManager.MenuType.MainGame);
	}
}
