using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/**
 * \class DungeonHandler
 * \brief Manages the dungeon layout, room transitions, and player movement between rooms.
 *
 * This class is responsible for handling the overall structure of the dungeon,
 * including loading rooms, managing room transitions, and keeping track of the
 * player's position within the dungeon.
 */
public partial class DungeonHandler : Node
{
	[Export]
	public int MinRooms = 5; ///< Minimum number of rooms in the dungeon.
	[Export]
	public int MaxRooms = 10; ///< Maximum number of rooms in the dungeon.
	
	private Dungeon Dungeon; ///< The current dungeon instance.
	private RoomHandler RoomHandler; ///< Reference to the RoomHandler node.
	private Node2D Player; ///< Reference to the player node.
	private bool _debugQuickClear = false; // Set to true to instantly clear rooms for testing

	/**
	 * Called when the node is added to the scene tree, adds this node to the dungeon_handler group.
	 */
	public override void _EnterTree()
	{
		AddToGroup("dungeon_handler");
	}

	/**
	 * Called when the node enters the scene tree for the first time.
	 * Initializes the dungeon, player, and room handler, and loads the initial room.
	 */
	public override void _Ready()
	{
		Player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		SetDungeon(Dungeons.IntroDungeon);
		
		// Subscribe to menu events
		MenuManager menuManager = GetTree().GetFirstNodeInGroup("menu_manager") as MenuManager;
		if (menuManager != null)
		{
			menuManager.MenuChanged += OnMenuChanged;
			menuManager.MenuLeft += OnMenuLeft;
		}
	}

	/**
	 * Loads a dungeon.
	 *
	 * @param dungeon The dungeon to set.
	 */
	public void SetDungeon(Dungeon dungeon)
	{
		Dungeon = new Dungeon(dungeon);
	}

	public void LoadFirstRoom()
	{
		LoadRoom(Dungeon.CurrentRoomPosition, Direction.DOWN);
	}

	/**
	Reloads the dungeon by relabeling all rooms as not cleared and loading again into the first room
	*/
	public void ReloadDungeon()
	{	
		// reset the rooms
		foreach (Room room in Dungeon.Layout.Values)
		{
			room.IsCleared = false;
			room.IsVisited = false;
		}

		LoadRoom(Dungeon.EntrancePosition, Direction.DOWN); // load into the first room
	}

	private void CheckRoomHandler()
	{
		if (RoomHandler == null)
		{
			RoomHandler = GetTree().GetFirstNodeInGroup("room_handler") as RoomHandler;
		}
	}

	/**
	 * Loads a room at the specified position and sets the player's entrance direction.
	 * 
	 * @param position The position of the room in the dungeon layout.
	 * @param enterDirection The direction from which the player enters the room.
	 */
	private void LoadRoom(Vector2I position, Direction enterDirection)
	{
		if (!Dungeon.Layout.ContainsKey(position))
		{
			GD.PrintErr($"Attempted to load non-existent room at position {position}");
			return;
		}

		Room room = Dungeon.Layout[position];

		GD.Print($"Loading room: {room} at position {position}, roomHandler is null: {RoomHandler == null}");

		RoomHandler.LoadRoom(room, enterDirection);
		room.IsVisited = true;
		Dungeon.Layout[position] = room;  // Update the room in the dictionary
		Dungeon.CurrentRoomPosition = position;

		ConnectRoomExitSignals();
		
		// Auto-clear room if in debug mode
		if (_debugQuickClear)
		{
			CallDeferred(nameof(QuickClearCurrentRoom));
		}
		
		// if the room is the boss room. Also connect the dungeon clear signal
		if (position == Dungeon.BossPosition)
		{
			RoomHandler.RoomCleared += () => CallDeferred(nameof(OnDungeonCompleted));
		}
	}

	/**
	 * Connects signals for room exits in the current room.
	 */
	private void ConnectRoomExitSignals()
	{
		foreach (Node child in RoomHandler.CurrentRoomNode.GetChildren())
		{
			if (child is RoomExit exit)
			{
				exit.PlayerEnteredDoor += OnPlayerEnteredDoor;
			}
		}
	}

	/**
	 * Calculates the direction from which the player enters a new room.
	 * 
	 * @param newPosition The position of the new room.
	 * @return The direction from which the player enters the new room.
	 */
	private Direction CalculateEnterDirection(Vector2I newPosition)
	{
		Vector2I delta = newPosition - Dungeon.CurrentRoomPosition;
		if (delta.X > 0) return Direction.LEFT;
		if (delta.X < 0) return Direction.RIGHT;
		if (delta.Y > 0) return Direction.UP;
		if (delta.Y < 0) return Direction.DOWN;
		return Direction.RIGHT;  // Default direction if same position
	}

	/**
	 * Handles the player entering a door to transition to a new room.
	 * 
	 * @param direction The direction of the door the player entered.
	 */
	private void OnPlayerEnteredDoor(Direction direction)
	{
		Vector2I newPosition = DirectionHelper.CalculateNewPosition(Dungeon.CurrentRoomPosition, direction);
		
		if (Dungeon.Layout.ContainsKey(newPosition))
		{
			LoadRoom(newPosition, DirectionHelper.GetOppositeDirection(direction));
		}
		else
		{
			GD.PrintErr($"No room exists in direction {direction} from current room");
		}
	}
	
	/**
	 * Gets the current room position in the dungeon layout.
	 * 
	 * @return The current room position.
	 */
	public Vector2I GetCurrentRoomPosition()
	{
		return Dungeon.CurrentRoomPosition;
	}

	/**
	 * Gets the grid size of the dungeon.
	 * 
	 * @return The grid size of the dungeon.
	 */
	public Vector2 GetGridSize()
	{
		return Dungeon.GridSize;
	}
	
	/**
	 * Gets the entire dungeon layout.
	 * 
	 * @return A dictionary representing the dungeon layout.
	 */
	public Dictionary<Vector2I, Room> GetDungeonLayout()
	{
		return Dungeon.Layout;
	}

	/**
	 * Gets the magic type of the dungeon.
	 * 
	 * @return The magic type of the dungeon.
	 */
	public MagicType GetMagicType()
	{
		return Dungeon.MagicType;
	}

	private void OnMenuChanged(MenuManager.MenuType newMenu, bool isPush)
	{
		GD.Print($"Menu changed: {newMenu} {isPush}");
		if (newMenu == MenuManager.MenuType.MainGame)
		{
			RoomHandler = GetTree().GetFirstNodeInGroup("room_handler") as RoomHandler;
			GD.Print($"RoomHandler: {RoomHandler == null}");
			if(Dungeon != null)
			{
				LoadFirstRoom();
			}
		}
	}

	private void OnMenuLeft(MenuManager.MenuType leftMenu)
	{
		if (leftMenu == MenuManager.MenuType.MainGame)
		{
			RoomHandler = null;
		}
	}

	public override void _ExitTree()
	{
		MenuManager menuManager = GetTree().GetFirstNodeInGroup("menu_manager") as MenuManager;
		if (menuManager != null)
		{
			menuManager.MenuChanged -= OnMenuChanged;
			menuManager.MenuLeft -= OnMenuLeft;
		}
	}

	/**
	 * Opens the dungeon clear menu.
	 */
	private void OpenDungeonClearMenu()
	{
		MenuManager menuManager = GetTree().GetFirstNodeInGroup("menu_manager") as MenuManager;
		menuManager.PushMenu(MenuManager.MenuType.DungeonClearMenu);
	}

	/**
	 * Is called after all enemies of the boss room are killed
	 */
	public void OnDungeonCompleted()
	{
		OpenDungeonClearMenu();

		var progressManager = GetTree().GetFirstNodeInGroup("progress_manager") as ProgressManager;
		
		if (Dungeon.Name == "Intro")
		{
			progressManager.CompleteIntroDungeon();
		}
		else if (Dungeon.IsStoryDungeon)
		{
			progressManager.CompleteStoryDungeon(Dungeon.StoryIndex);
		}
	}

	/**
	 * Quickly toggles debug mode.
	 *
	 * @param enabled True to enable debug mode, false to disable it.
	 */
	public void ToggleQuickClear(bool enabled)
	{
		_debugQuickClear = enabled;
		GD.Print($"Quick clear mode: {_debugQuickClear}");
	}

	/**
	 * Instantly clears the current room.
	 */
	private void QuickClearCurrentRoom()
	{
		// Wait a frame to ensure room is fully loaded
		GetTree().CreateTimer(0.1f).Timeout += () =>
		{
			// Force room to be cleared
			Room currentRoom = Dungeon.Layout[Dungeon.CurrentRoomPosition];
			currentRoom.IsCleared = true;
			Dungeon.Layout[Dungeon.CurrentRoomPosition] = currentRoom;
			
			// Enable exits
			RoomHandler.EnableRoomExits();
			
			// If this is the boss room, complete the dungeon
			if (Dungeon.CurrentRoomPosition == Dungeon.BossPosition)
			{
				OnDungeonCompleted();
			}
		};
	}

}
