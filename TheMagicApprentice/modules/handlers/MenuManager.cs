using Godot;
using System;
using System.Collections.Generic;

public partial class MenuManager : Node
{
	[Signal]
	public delegate void MenuChangedEventHandler(MenuType newMenu, bool isPush);

	public enum MenuType
	{
		MainGame,
		MainMenu,
		MainHub,
		PauseMenu,
		SettingsMenu
	}

	private static readonly Dictionary<MenuType, string> MenuScenePaths = new()
	{
		{ MenuType.MainGame, "res://main_game.tscn" },
		{ MenuType.MainMenu, "res://modules/ui/main_menu/main_menu.tscn" },
		{ MenuType.MainHub, "res://modules/ui/main_hub/main_hub.tscn" },
		{ MenuType.PauseMenu, "res://modules/ui/pause_menu/pause_menu.tscn" },
		{ MenuType.SettingsMenu, "res://modules/ui/settings_menu/settings_menu.tscn" },
		// Add other menu types and their scene paths here
	};

	private static readonly Dictionary<MenuType, bool> UsesCanvasLayer = new()
	{
		{ MenuType.MainGame, false },
		{ MenuType.MainMenu, true },
		{ MenuType.MainHub, true },
		{ MenuType.PauseMenu, true },
		{ MenuType.SettingsMenu, true }
	};
	
	public MenuType CurrentMenu => _menuStack.Count > 0 ? _menuStack.Peek() : MenuType.MainMenu;
	private Stack<MenuType> _menuStack = new();
	private Dictionary<MenuType, Node> _menuNodes = new();

	public override void _Ready()
	{
		PushMenu(MenuType.MainMenu);
	}

	public void PushMenu(MenuType newMenu)
	{
		GD.Print(newMenu);
		if (!_menuStack.Contains(newMenu))
		{
			if (_menuStack.Count > 0)
			{
				FreezeCurrentMenu();
			}
			_menuStack.Push(newMenu);
			EmitSignal(SignalName.MenuChanged, (int)newMenu, true);
			InstantiateMenuOverlay(newMenu);
		}
	}

	public void PopMenu()
	{
		if (_menuStack.Count > 1)
		{
			MenuType poppedMenu = _menuStack.Pop();
			if (_menuNodes.TryGetValue(poppedMenu, out Node poppedNode))
			{
				poppedNode.QueueFree();
				_menuNodes.Remove(poppedMenu);
			}
			GetTree().CreateTimer(0.1f).Connect("timeout", Callable.From(() => UnfreezeCurrentMenu()));
			EmitSignal(SignalName.MenuChanged, (int)CurrentMenu, false);
		}
	}

	private void InstantiateMenuOverlay(MenuType menuType)
	{
		if (MenuScenePaths.TryGetValue(menuType, out string scenePath))
		{
			var menuScene = GD.Load<PackedScene>(scenePath);
			var menuInstance = menuScene.Instantiate();
			GetTree().Root.CallDeferred("add_child", menuInstance);
			_menuNodes[menuType] = menuInstance;
		}
	}

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
