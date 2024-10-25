using Godot;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System;

public partial class DungeonSelection : BaseMenu
{
	private VBoxContainer storyDungeonsContainer;
	private VBoxContainer customDungeonsContainer;
	private Button storyButton;
	private Button customButton;
	private DungeonHandler dungeonHandler;
	private List<Dungeon> dungeons;

	public override void _Ready()
	{
		MenuType = MenuManager.MenuType.DungeonSelection;
		base._Ready();

		dungeonHandler = GetTree().GetFirstNodeInGroup("dungeon_handler") as DungeonHandler;
		System.Diagnostics.Debug.Assert(dungeonHandler != null);

		// Get references to containers and buttons
		storyDungeonsContainer = GetNode<VBoxContainer>("%StoryDungeonsContainer");
		customDungeonsContainer = GetNode<VBoxContainer>("%CustomDungeonsContainer");
		storyButton = GetNode<Button>("%StoryButton");
		customButton = GetNode<Button>("%CustomButton");

		// Connect button signals
		storyButton.Connect("pressed", new Callable(this, nameof(OnStoryButtonPressed)));
		customButton.Connect("pressed", new Callable(this, nameof(OnCustomButtonPressed)));

		// Load and populate dungeon buttons
		LoadDungeonButtons();

		// Show story dungeons by default
		ShowStoryDungeons();
	}

	private void LoadDungeonButtons()
	{
		// Clear existing buttons
		ClearContainer(storyDungeonsContainer);

		// Load story dungeons
		string jsonPath = "res://game_configuration/story_dungeons.json";
		List<Dungeon> dungeons = DungeonHelper.LoadDungeonsFromFile(jsonPath);

		foreach (var dungeon in dungeons)
		{
			Button dungeonButton = new Button();
			dungeonButton.Text = dungeon.Name;
			dungeonButton.Connect("pressed", Callable.From(() => OnDungeonButtonPressed(dungeon)));
			storyDungeonsContainer.AddChild(dungeonButton);
		}
	}

	private void ClearContainer(VBoxContainer container)
	{
		foreach (Node child in container.GetChildren())
		{
			container.RemoveChild(child);
			child.QueueFree();
		}
	}

	private void OnStoryButtonPressed()
	{
		ShowStoryDungeons();
	}

	private void OnCustomButtonPressed()
	{
		ShowCustomDungeons();
	}

	private void ShowStoryDungeons()
	{
		storyDungeonsContainer.Visible = true;
		customDungeonsContainer.Visible = false;
		storyButton.Disabled = true;
		customButton.Disabled = false;
	}

	private void ShowCustomDungeons()
	{
		storyDungeonsContainer.Visible = false;
		customDungeonsContainer.Visible = true;
		storyButton.Disabled = false;
		customButton.Disabled = true;
	}

	private void OnDungeonButtonPressed(Dungeon dungeon)
	{
		GD.Print($"Selected dungeon: {dungeon.Name}");
		dungeonHandler.SetDungeon(dungeon);
		SetRootMenu(MenuManager.MenuType.MainGame);
	}

	private void OnBackButtonPressed()
	{
		PopMenu();
	}
}

public class DungeonInfo
{
	public string name { get; set; }
	// Add other properties as needed
}
