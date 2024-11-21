using Godot;
using System;

public partial class NewGameMenu : BaseMenu
{
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
		SelectElementInPlayer(MagicType.SUN);
		CallDeferred(nameof(EnterIntroDungeon));
	}

	/**
	Gets called when the button for the cosmic element is pressed
	*/
	public void CosmicSelected()
	{
		SelectElementInPlayer(MagicType.COSMIC);
		CallDeferred(nameof(EnterIntroDungeon));
	}

	/**
	Gets called when the button for the dark element is pressed
	*/
	public void DarkSelected()
	{
		SelectElementInPlayer(MagicType.DARK);
		CallDeferred(nameof(EnterIntroDungeon));
	}

	/**
	Unlocks and selects the base spell of MagicType magicType in the player
	*/
	private void SelectElementInPlayer(MagicType magicType)
	{
		(GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player).SetStartBasic(magicType);
	}

	/**
	Exits this scene and loads into the IntroDungeon.
	For now only goes to MainHub
	*/
	private void EnterIntroDungeon()
	{
		SetRootMenu(MenuManager.MenuType.MainHub);
	}
}
