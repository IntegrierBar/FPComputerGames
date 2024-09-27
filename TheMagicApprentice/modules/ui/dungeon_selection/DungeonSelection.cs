using Godot;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;

public partial class DungeonSelection : BaseMenu
{
	private VBoxContainer storyDungeonsContainer;
	private VBoxContainer customDungeonsContainer;
	private Button storyButton;
	private Button customButton;

	public override void _Ready()
	{
		MenuType = MenuManager.MenuType.DungeonSelection;
		base._Ready();

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
		string jsonContent = File.ReadAllText(ProjectSettings.GlobalizePath(jsonPath));
		List<DungeonInfo> dungeons = JsonSerializer.Deserialize<List<DungeonInfo>>(jsonContent);

		foreach (var dungeon in dungeons)
		{
			Button dungeonButton = new Button();
			dungeonButton.Text = dungeon.name;
			dungeonButton.Connect("pressed", Callable.From(() => OnDungeonButtonPressed(dungeon.name)));
			storyDungeonsContainer.AddChild(dungeonButton);
		}

		// TODO: Load custom dungeons (implement this part when you have custom dungeons)
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

	private void OnDungeonButtonPressed(string dungeonName)
	{
		GD.Print($"Selected dungeon: {dungeonName}");
			// Add logic here to start the selected dungeon
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
