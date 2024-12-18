using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/**
 * Manages the menu system for the game.
 * Handles menu transitions, stack management, and menu instantiation.
 */
public partial class MenuManager : Node
{
	[Signal]
	public delegate void MenuChangedEventHandler(MenuType newMenu, bool isPush);
	[Signal]
	public delegate void MenuLeftEventHandler(MenuType leftMenu);

	/**
	 * Enum representing different types of menus in the game.
	 */
	public enum MenuType
	{
		MainGame,
		MainMenu,
		MainHub,
		PauseMenu,
		SettingsMenu,
		DungeonSelection,
		DungeonClearMenu,
		NewGameMenu,
		PlayerDeathMenu,
	}

	/**
	 * Dictionary mapping menu types to their corresponding scene paths.
	 */
	private static readonly Dictionary<MenuType, string> MenuScenePaths = new()
	{
		{ MenuType.MainGame, "res://main_game.tscn" },
		{ MenuType.MainMenu, "res://modules/ui/main_menu/main_menu.tscn" },
		{ MenuType.MainHub, "res://modules/ui/main_hub/main_hub.tscn" },
		{ MenuType.PauseMenu, "res://modules/ui/pause_menu/pause_menu.tscn" },
		{ MenuType.SettingsMenu, "res://modules/ui/settings_menu/settings_menu.tscn" },
		{ MenuType.DungeonSelection, "res://modules/ui/dungeon_selection/dungeon_selection.tscn" },
		{ MenuType.DungeonClearMenu, "res://modules/ui/dungeon_clear_menu/dungeon_clear_menu.tscn" },
		{ MenuType.NewGameMenu, "res://modules/ui/new_game_menu/new_game_menu.tscn"},
		{ MenuType.PlayerDeathMenu, "res://modules/ui/player_death_menu/player_death_menu.tscn"},
		// Add other menu types and their scene paths here
	};
	
	/**
	 * Gets the current active menu.
	 */
	public MenuType CurrentMenu => _menuStack.Count > 0 ? _menuStack.Peek() : MenuType.MainMenu;

	private Stack<MenuType> _menuStack = new();
	private Dictionary<MenuType, Node> _menuNodes = new();

	

	/**
	 * Called when the node is added to the scene tree, adds this node to the menu_manager group.
	 */
	public override void _EnterTree()
	{
		AddToGroup("menu_manager");
	}

	/**
	 * Pushes a new menu onto the stack and instantiates it.
	 * 
	 * @param newMenu The type of menu to push onto the stack.
	 */
	public void PushMenu(MenuType newMenu)
	{
		if (!_menuStack.Contains(newMenu))
		{
			if (_menuStack.Count > 0)
			{
				FreezeCurrentMenu();
			}
			_menuStack.Push(newMenu);
			InstantiateMenuOverlay(newMenu);
			EmitSignal(SignalName.MenuChanged, (int)newMenu, true);
		}
	}

	/**
	 * Pops the top menu from the stack and removes its instance.
	 */
	public void PopMenu()
	{
		if (_menuStack.Count > 1)
		{
			MenuType poppedMenu = _menuStack.Pop();
			if (_menuNodes.TryGetValue(poppedMenu, out Node poppedNode))
			{
				poppedNode.QueueFree();
				_menuNodes.Remove(poppedMenu);
				EmitSignal(SignalName.MenuLeft, (int)poppedMenu);
			}
			GetTree().CreateTimer(0.1f).Connect("timeout", Callable.From(() => UnfreezeCurrentMenu()));
			EmitSignal(SignalName.MenuChanged, (int)CurrentMenu, false);
		}
	}

	/**
	 * Clears the menu stack and sets a new root menu.
	 * 
	 * @param newMenu The type of menu to set as the new root.
	 */
	public void SetRootMenu(MenuType newMenu)
	{
		while (_menuStack.Count > 0)
		{
			MenuType poppedMenu = _menuStack.Pop();
			if (_menuNodes.TryGetValue(poppedMenu, out Node poppedNode))
			{
				poppedNode.QueueFree();
				_menuNodes.Remove(poppedMenu);
			}
		}
		PushMenu(newMenu);
	}

	public void RequestRootMenu(BaseMenu menu)
	{
		if (_menuStack.Count == 0)
		{
			// Validate that we're actually getting a BaseMenu
			if (menu is not BaseMenu)
			{
				GD.PrintErr($"Invalid menu type requested: {menu.GetType()}");
				return;
			}

			menu.CallDeferred("reparent", this);
			_menuNodes[menu.MenuType] = menu;
			_menuStack.Push(menu.MenuType);
			EmitSignal(SignalName.MenuChanged, (int)menu.MenuType, true);
		}
	}

	/**
	 * Instantiates a menu overlay based on the given menu type.
	 * 
	 * @param menuType The type of menu to instantiate.
	 */
	private void InstantiateMenuOverlay(MenuType menuType)
	{
		if (MenuScenePaths.TryGetValue(menuType, out string scenePath))
		{
			var menuScene = GD.Load<PackedScene>(scenePath);
			var menuInstance = menuScene.Instantiate();
			AddChild(menuInstance);
			_menuNodes[menuType] = menuInstance;
		}
	}

	/**
	 * Freezes the current menu by disabling its processing.
	 */
	private void FreezeCurrentMenu()
	{
		if (_menuStack.Count > 0)
		{
			MenuType currentMenu = _menuStack.Peek();
			if (_menuNodes.TryGetValue(currentMenu, out Node currentNode))
			{
				currentNode.ProcessMode = ProcessModeEnum.Disabled;
			}
		}
	}

	/**
	 * Unfreezes the current menu by enabling its processing.
	 */
	private void UnfreezeCurrentMenu()
	{
		if (_menuStack.Count > 0)
		{
			MenuType currentMenu = _menuStack.Peek();
			if (_menuNodes.TryGetValue(currentMenu, out Node currentNode))
			{
				currentNode.ProcessMode = ProcessModeEnum.Inherit;
			}
		}
	}
}
