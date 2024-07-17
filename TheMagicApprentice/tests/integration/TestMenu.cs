namespace Tests;

using GdUnit4;
using Godot;
using System;
using System.Collections.Generic;
using static GdUnit4.Assertions;
using System.Threading.Tasks;

[TestSuite]
public partial class TestMenuManager
{
	private ISceneRunner _sceneRunner;
	private MenuManager _menuManager;

	[BeforeTest]
	public void SetupTest()
	{
		GD.Print("Setting up test environment...");

		// Load the scene using ISceneRunner
		_sceneRunner = ISceneRunner.Load("res://tests/integration/test_main_menu.tscn");
		GD.Print("Scene loaded.");

		_menuManager = _sceneRunner.FindChild("MenuManager") as MenuManager;
	}

	[AfterTest]
	public void TeardownTest()
	{
		GD.Print("Tearing down test environment...");

		// Clean up the scene runner
		_sceneRunner = null;

		_menuManager = null;

		GD.Print("Scene runner and nodes freed.");
	}

	[TestCase]
	public void TestMenuStackOperations()
	{
		// Test initial state
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.MainMenu);

		// Test pushing a new menu
		_menuManager.PushMenu(MenuManager.MenuType.PauseMenu);
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.PauseMenu);

		// Test pushing another menu
		_menuManager.PushMenu(MenuManager.MenuType.SettingsMenu);
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.SettingsMenu);

		// Test popping a menu
		_menuManager.PopMenu();
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.PauseMenu);

		// Test popping back to the root menu
		_menuManager.PopMenu();
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.MainMenu);

		// Test that we can't pop the root menu
		_menuManager.PopMenu();
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.MainMenu);
	}

	[TestCase]
	public void TestSetRootMenu()
	{
		// Push some menus onto the stack
		_menuManager.PushMenu(MenuManager.MenuType.PauseMenu);
		_menuManager.PushMenu(MenuManager.MenuType.SettingsMenu);

		// Set a new root menu
		_menuManager.SetRootMenu(MenuManager.MenuType.MainHub);

		// Check that the new root menu is set and is the only menu in the stack
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.MainHub);

		// Try to pop the menu, it should remain as the root
		_menuManager.PopMenu();
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.MainHub);
	}

	[TestCase]
	public async Task TestMenuInstantiation()
	{
		// Push a new menu and check if it's instantiated
		_menuManager.PushMenu(MenuManager.MenuType.PauseMenu);
		var pauseMenu = _menuManager.GetNode("PauseMenu");
		AssertThat(pauseMenu).IsNotNull();

		// Push another menu and check if both are present
		_menuManager.PushMenu(MenuManager.MenuType.SettingsMenu);
		var settingsMenu = _menuManager.GetNode("SettingsMenu");
		AssertThat(settingsMenu).IsNotNull();
		AssertThat(pauseMenu).IsNotNull();
		// Pop a menu and check if it's removed
		_menuManager.PopMenu();
		await _sceneRunner.SimulateFrames(10, 20); // Wait for 10 frames with 20ms per frame
		settingsMenu = _menuManager.GetNodeOrNull("SettingsMenu");
		AssertThat(settingsMenu).IsNull();
		AssertThat(pauseMenu).IsNotNull();
	}

	[TestCase]
	public async Task TestMenuFreezingAndUnfreezing()
	{
		// Push a menu and check its process mode
		_menuManager.PushMenu(MenuManager.MenuType.PauseMenu);
		await _sceneRunner.SimulateFrames(10, 20); // Wait for 10 frames with 20ms per frame
		var pauseMenu = _menuManager.GetNode("PauseMenu");
		AssertThat(pauseMenu.ProcessMode).IsEqual(Node.ProcessModeEnum.Inherit);

		// Push another menu and check if the previous one is frozen
		_menuManager.PushMenu(MenuManager.MenuType.SettingsMenu);
		await _sceneRunner.SimulateFrames(10, 20); // Wait for 10 frames with 20ms per frame
		AssertThat(pauseMenu.ProcessMode).IsEqual(Node.ProcessModeEnum.Disabled);

		// Pop the top menu and check if the previous one is unfrozen
		_menuManager.PopMenu();
		await _sceneRunner.SimulateFrames(10, 20); // Wait for 10 frames with 20ms per frame
		AssertThat(pauseMenu.ProcessMode).IsEqual(Node.ProcessModeEnum.Inherit);
	}

	[TestCase]
	public async Task TestMainGameMenuTransition()
	{
		// Start with the main menu
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.MainMenu);

		// Transition to the main game
		_menuManager.SetRootMenu(MenuManager.MenuType.MainGame);
		await _sceneRunner.SimulateFrames(10, 20); // Wait for 10 frames with 20ms per frame

		// Check if the current menu is MainGame
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.MainGame);

		// Check if the MainGame scene is instantiated
		var mainGame = _menuManager.GetNode("Scene");
		AssertThat(mainGame).IsNotNull();
		AssertThat(mainGame).IsInstanceOf<Node2D>();

		// Check if the MainGame script is attached and functioning
		var mainGameScript = mainGame.GetNode<CanvasLayer>("CanvasLayer");
		AssertThat(mainGameScript).IsNotNull();
		AssertThat(mainGameScript).IsInstanceOf<MainGame>();

		// Simulate pressing the ESC key to open the pause menu
		var escEvent = new InputEventKey
		{
			Keycode = Key.Escape,
			Pressed = true
		};
		InputMap.ActionAddEvent("esc", escEvent);
		Input.ParseInputEvent(escEvent);
		await _sceneRunner.SimulateFrames(10, 20); // Wait for 10 frames with 20ms per frame

		// Check if the pause menu is now the current menu
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.PauseMenu);

		// Check if both MainGame and PauseMenu are present in the scene
		AssertThat(mainGame).IsNotNull();
		var pauseMenu = _menuManager.GetNode("PauseMenu");
		AssertThat(pauseMenu).IsNotNull();

		// Pop the pause menu
		_menuManager.PopMenu();
		await _sceneRunner.SimulateFrames(10, 20); // Wait for 10 frames with 20ms per frame

		// Check if we're back to the MainGame menu
		AssertThat(_menuManager.CurrentMenu).IsEqual(MenuManager.MenuType.MainGame);
	}
}
