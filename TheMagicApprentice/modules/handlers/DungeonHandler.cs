using Godot;
using System;
using System.Collections.Generic;

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
	
	private Dungeon dungeon; ///< The current dungeon instance.
	private RoomHandler roomHandler; ///< Reference to the RoomHandler node.
	private Node2D player; ///< Reference to the player node.

	/**
	 * Called when the node enters the scene tree for the first time.
	 * Initializes the dungeon, player, and room handler, and loads the initial room.
	 */
	public override void _Ready()
	{
		player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		roomHandler = GetTree().GetFirstNodeInGroup("room_handler") as RoomHandler;
		//dungeon = new Dungeon(MinRooms, MaxRooms);
		dungeon = new Dungeon(Dungeons.IntroDungeon);
		LoadRoom(dungeon.CurrentRoomPosition, Direction.DOWN);
	}

	/**
	 * Loads a room at the specified position and sets the player's entrance direction.
	 * 
	 * @param position The position of the room in the dungeon layout.
	 * @param enterDirection The direction from which the player enters the room.
	 */
	private void LoadRoom(Vector2I position, Direction enterDirection)
	{
		if (!dungeon.Layout.ContainsKey(position))
		{
			GD.PrintErr($"Attempted to load non-existent room at position {position}");
			return;
		}

		Room room = dungeon.Layout[position];

		roomHandler.LoadRoom(room, enterDirection);
		room.IsVisited = true;
		dungeon.Layout[position] = room;  // Update the room in the dictionary
		dungeon.CurrentRoomPosition = position;

		ConnectRoomExitSignals();
	}

	/**
	 * Connects signals for room exits in the current room.
	 */
	private void ConnectRoomExitSignals()
	{
		foreach (Node child in roomHandler.CurrentRoomNode.GetChildren())
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
		Vector2I delta = newPosition - dungeon.CurrentRoomPosition;
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
		Vector2I newPosition = CalculateNewPosition(direction);
		
		if (dungeon.Layout.ContainsKey(newPosition))
		{
			LoadRoom(newPosition, DirectionHelper.GetOppositeDirection(direction));
		}
		else
		{
			GD.PrintErr($"No room exists in direction {direction} from current room");
		}
	}

	/**
	 * Calculates the new position in the dungeon layout based on the direction of movement out of the current room.
	 * 
	 * @param direction The direction of movement.
	 * @return The new position in the dungeon layout.
	 */
	private Vector2I CalculateNewPosition(Direction direction)
	{
		Vector2I newPosition = dungeon.CurrentRoomPosition;
		switch (direction)
		{
			case Direction.UP: newPosition.Y--; break;
			case Direction.DOWN: newPosition.Y++; break;
			case Direction.LEFT: newPosition.X--; break;
			case Direction.RIGHT: newPosition.X++; break;
		}
		return newPosition;
	}
	
	/**
	 * Gets the current room position in the dungeon layout.
	 * 
	 * @return The current room position.
	 */
	public Vector2I GetCurrentRoomPosition()
	{
		return dungeon.CurrentRoomPosition;
	}

	/**
	 * Gets the grid size of the dungeon.
	 * 
	 * @return The grid size of the dungeon.
	 */
	public Vector2 GetGridSize()
	{
		return dungeon.GridSize;
	}
	
	/**
	 * Gets the entire dungeon layout.
	 * 
	 * @return A dictionary representing the dungeon layout.
	 */
	public Dictionary<Vector2I, Room> GetDungeonLayout()
	{
		return dungeon.Layout;
	}
}
