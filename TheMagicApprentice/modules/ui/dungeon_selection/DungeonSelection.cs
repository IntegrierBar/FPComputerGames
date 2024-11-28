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

	private bool magicTypeSelected = false; //< Needed because default value of selectedMagicType is 0 (Sun)
	private MagicType selectedMagicType;
	private MagicTypeSelectionButton[] magicTypeButtons;

	private int _remainingRerolls = 2;
	private Button _generateButton;
	private Button _rerollButton;
	private Label _cursesLabel;
	private VBoxContainer _curseContainer;
	private SkillTree _skillTree;

	public override void _Ready()
	{
		MenuType = MenuManager.MenuType.DungeonSelection;
		base._Ready();

		var player = GetTree().GetFirstNodeInGroup(Globals.PlayerGroup) as Player;
		_skillTree = player.GetNode<SkillTree>("SkillTree");

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

		// Get references to the three magic type buttons
		magicTypeButtons = new[]
		{
			GetNode<MagicTypeSelectionButton>("%SunButton"),
			GetNode<MagicTypeSelectionButton>("%CosmicButton"),
			GetNode<MagicTypeSelectionButton>("%DarkButton")
		};

		// Connect signals for all magic type buttons
		foreach (var button in magicTypeButtons)
		{
			button.Connect(MagicTypeSelectionButton.SignalName.MagicTypeSelected, 
				new Callable(this, nameof(OnMagicTypeSelected)));
		}

		// Get references to new UI elements
		_generateButton = GetNode<Button>("%GenerateButton");
		_rerollButton = GetNode<Button>("%RerollButton");
		_cursesLabel = GetNode<Label>("%CursesLabel");
		_curseContainer = GetNode<VBoxContainer>("%CurseContainer");

		// Connect signals
		_generateButton.Connect("pressed", new Callable(this, nameof(OnGenerateButtonPressed)));
		_rerollButton.Connect("pressed", new Callable(this, nameof(OnRerollButtonPressed)));

		UpdateCurseDisplay();
		UpdateRerollButton();
	}

	private void LoadDungeonButtons()
	{
		// Clear existing buttons
		ClearContainer(storyDungeonsContainer);

		// Get ProgressManager to check dungeon availability
		var progressManager = GetTree().GetFirstNodeInGroup(Globals.ProgressManagerGroup) as ProgressManager;

		// Load story dungeons
		string jsonPath = "res://game_configuration/story_dungeons.json";
		List<Dungeon> dungeons = DungeonHelper.LoadDungeonsFromFile(jsonPath);

		foreach (var dungeon in dungeons)
		{
			// Skip intro dungeon - it should only be accessible from NewGameMenu
			if (dungeon.Name == "Intro")
				continue;

			Button dungeonButton = new Button();
			dungeonButton.Text = dungeon.Name;
			dungeonButton.Connect("pressed", Callable.From(() => OnDungeonButtonPressed(dungeon)));

			if (dungeon.IsStoryDungeon)
			{
				// Story dungeons are only available if:
				// 1. Intro is completed
				// 2. All previous story dungeons are completed
				// 3. It's the next dungeon in sequence
				bool isAvailable = progressManager.IsIntroDungeonCompleted() && 
								 dungeon.StoryIndex == progressManager.GetCurrentStoryDungeonIndex();

				dungeonButton.Disabled = !isAvailable;

				// Optional: Add tooltip to explain why dungeon is locked
				if (!isAvailable)
				{
					if (!progressManager.IsIntroDungeonCompleted())
					{
						dungeonButton.TooltipText = "Complete the Intro dungeon first";
					}
					else
					{
						dungeonButton.TooltipText = "Complete previous dungeons first";
					}
				}

				// Optional: Add visual indication of completion status
				if (progressManager.IsStoryDungeonCompleted(dungeon.StoryIndex))
				{
					dungeonButton.Text += " (Completed)";
					// You could also add an icon or change the button's theme here
				}
			}

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
		customButton.Disabled = !_skillTree.IsAnyBasicSpellUnlocked();
		if(!_skillTree.IsAnyBasicSpellUnlocked())
		{
			customButton.Text = "Custom (Locked)";
			customButton.TooltipText = "Unlock a basic spell first";
			customButton.Modulate = new Color(1, 1, 1, 0.5f);
		}
	}

	private void ShowCustomDungeons()
	{
		storyDungeonsContainer.Visible = false;
		customDungeonsContainer.Visible = true;
		storyButton.Disabled = false;
		customButton.Disabled = !_skillTree.IsAnyBasicSpellUnlocked();
		if(!_skillTree.IsAnyBasicSpellUnlocked())
		{
			customButton.Text = "Custom (Locked)";
			customButton.TooltipText = "Unlock a basic spell first";
			customButton.Modulate = new Color(1, 1, 1, 0.5f);
		}
	}

	private void OnDungeonButtonPressed(Dungeon dungeon)
	{
		GD.Print($"Selected dungeon: {dungeon.Name}");

		// If no magic type selected, get random unlocked basic spell and convert to magic type
		if (selectedMagicType == MagicType.SUN) // Default value
		{
			SpellName randomBasicSpell = _skillTree.GetRandomUnlockedBasicSpell();
			dungeon.MagicType = EntityTypeHelper.GetMagicTypeOfSpell(randomBasicSpell);
		}
		else
		{
			dungeon.MagicType = selectedMagicType;
		}

		dungeonHandler.SetDungeon(dungeon);
		SetRootMenu(MenuManager.MenuType.MainGame);
	}

	private void OnBackButtonPressed()
	{
		PopMenu();
	}

	private void OnMagicTypeSelected(MagicType magicType)
	{
		selectedMagicType = magicType;
		GD.Print($"Selected magic type: {EntityTypeHelper.GetMagicTypeAsString(magicType)}");
		
		// Update button appearances (optional - if you want to show which is selected)
		foreach (var button in magicTypeButtons)
		{
			button.ButtonPressed = button.MagicType == selectedMagicType;
		}
	}

	private void OnGenerateButtonPressed()
	{
		// Create a new dungeon
		var dungeon = DungeonGenerator.GenerateDungeon(5, 10);
		
		// Set magic type based on selection
		if (selectedMagicType == MagicType.SUN) // Default value
		{
			SpellName randomBasicSpell = _skillTree.GetRandomUnlockedBasicSpell();
			dungeon.MagicType = EntityTypeHelper.GetMagicTypeOfSpell(randomBasicSpell);
		}
		else
		{
			dungeon.MagicType = selectedMagicType;
		}

		dungeonHandler.SetDungeon(dungeon);
		SetRootMenu(MenuManager.MenuType.MainGame);
	}

	private void OnRerollButtonPressed()
	{
		if (_remainingRerolls > 0)
		{
			_remainingRerolls--;
			RandomizeCurses();
			UpdateCurseDisplay();
			UpdateRerollButton();
		}
	}

	private void RandomizeCurses()
	{
		// Clear existing curses
		CurseHandler.ClearAllCurses();

		// Randomly activate 2-3 curses
		var allCurses = Enum.GetValues(typeof(Curse));
		var random = new Random();
		int numCurses = random.Next(2, 4); // 2 or 3 curses

		// Create a list of indices and shuffle it
		var indices = new List<int>();
		for (int i = 0; i < allCurses.Length; i++)
			indices.Add(i);
		
		for (int i = indices.Count - 1; i > 0; i--)
		{
			int j = random.Next(i + 1);
			(indices[i], indices[j]) = (indices[j], indices[i]);
		}

		// Activate the first numCurses curses
		for (int i = 0; i < numCurses; i++)
		{
			CurseHandler.ActivateCurse((Curse)allCurses.GetValue(indices[i]));
		}
	}

	private void UpdateCurseDisplay()
	{
		// Clear existing curse labels
		foreach (Node child in _curseContainer.GetChildren())
		{
			_curseContainer.RemoveChild(child);
			child.QueueFree();
		}

		foreach (Curse curse in Enum.GetValues(typeof(Curse)))
		{
			if (CurseHandler.IsActive(curse))
			{
				var label = new Label
				{
					Text = CurseHandler.GetCurseDescription(curse)
				};
				_curseContainer.AddChild(label);
			}
		}
	}

	private void UpdateRerollButton()
	{
		_rerollButton.Text = $"Reroll Curses ({_remainingRerolls} left)";
		_rerollButton.Disabled = _remainingRerolls <= 0;
	}

	public void RestoreRerolls(bool fullRestore = false)
	{
		_remainingRerolls = fullRestore ? 2 : Math.Min(_remainingRerolls + 1, 2);
		UpdateRerollButton();
	}
}

public class DungeonInfo
{
	public string name { get; set; }
	// Add other properties as needed
}
