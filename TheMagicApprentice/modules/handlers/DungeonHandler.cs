using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonHandler : Node
{
	[Export]
	public int MinRooms = 5;
	[Export]
	public int MaxRooms = 10;
	
	public Vector2I GridSize => new Vector2I(2 * MaxRooms + 1, 2 * MaxRooms + 1);

	private Dictionary<Vector2I, Room> dungeonLayout = new Dictionary<Vector2I, Room>();
	private Vector2I currentRoomPosition;
	private RoomHandler roomHandler;
	private Node2D player;

	public override void _Ready()
	{
		player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		roomHandler = GetTree().GetFirstNodeInGroup("room_handler") as RoomHandler;
		GenerateDungeon();
		LoadRoom(currentRoomPosition, Direction.DOWN);
	}

	private void GenerateDungeon()
	{
		dungeonLayout.Clear();

		int roomCount = GD.RandRange(MinRooms, MaxRooms);
		Vector2I entrancePos = new Vector2I(GridSize.X / 2, GridSize.Y / 2);
		Vector2I bossPos = entrancePos;
		currentRoomPosition = entrancePos;
		

		while (Math.Abs(entrancePos.X - bossPos.X) + Math.Abs(entrancePos.Y - bossPos.Y) < 4)
		{
			// Clear the current dungeon layout
			dungeonLayout = new Dictionary<Vector2I, Room>();

			// Set the entrance position
			dungeonLayout[entrancePos] = new Room(RoomType.Entry, GetRandomRoomScene());

			// Generate the rest of the rooms
			List<Vector2I> visitedTiles = new List<Vector2I> { entrancePos };
			Vector2I currentPos = entrancePos;
			for (int i = 0; i < roomCount - 1; i++)
			{
				Godot.Collections.Array directions = new Godot.Collections.Array{
					new Vector2I(1, 0),  // Move right
					new Vector2I(-1, 0), // Move left
					new Vector2I(0, 1),  // Move down
					new Vector2I(0, -1)  // Move up
				};
				directions.Shuffle();

				bool moved = false;
				foreach (Vector2I direction in directions)
				{
					Vector2I nextPos = currentPos + direction;
					nextPos.X = Math.Clamp(nextPos.X, 0, GridSize.X - 1);
					nextPos.Y = Math.Clamp(nextPos.Y, 0, GridSize.Y - 1);
					if (!dungeonLayout.ContainsKey(nextPos))
					{
						dungeonLayout[nextPos] = new Room(RoomType.Normal, GetRandomRoomScene());
						visitedTiles.Add(nextPos);
						currentPos = nextPos;
						moved = true;
						break;
					}
				}

				if (!moved)
				{
					// If no move was possible, pick a random visited tile and try again
					currentPos = visitedTiles[(int) GD.Randi() % visitedTiles.Count];
				}
			}

			bossPos = currentPos;
		}

		// Set the boss position
		dungeonLayout[bossPos] = new Room(RoomType.Boss, GetRandomRoomScene());
	}

	private string GetRandomRoomScene()
	{
		return GD.Randi() % 2 == 0 ? "res://modules/rooms/Room3.tscn" : "res://modules/rooms/Room4.tscn";
	}

	private void LoadRoom(Vector2I position, Direction enterDirection)
	{
		if (!dungeonLayout.ContainsKey(position))
		{
			GD.PrintErr($"Attempted to load non-existent room at position {position}");
			return;
		}

		Room room = dungeonLayout[position];

		roomHandler.LoadRoom(room.ScenePath, enterDirection);
		room.IsVisited = true;
		dungeonLayout[position] = room;  // Update the room in the dictionary
		currentRoomPosition = position;

		// Connect to RoomExit signals in the new room
		ConnectRoomExitSignals();
	}

	private void ConnectRoomExitSignals()
	{
		foreach (Node child in roomHandler.currentRoom.GetChildren())
		{
			if (child is RoomExit exit)
			{
				exit.PlayerEnteredDoor += OnPlayerEnteredDoor;
			}
		}
	}

	private Direction CalculateEnterDirection(Vector2I newPosition)
	{
		Vector2I delta = newPosition - currentRoomPosition;
		if (delta.X > 0) return Direction.LEFT;
		if (delta.X < 0) return Direction.RIGHT;
		if (delta.Y > 0) return Direction.UP;
		if (delta.Y < 0) return Direction.DOWN;
		return Direction.RIGHT;  // Default direction if same position
	}

	/**
	 * Callback for when the player enters a door. The DungeonHandler then loads a new room depending on the direction the player entered the exit.
	 * 
	 * @param targetRoom The name of the target room to load.
	 * @param direction The direction from which the player entered the door.
	 */
	private void OnPlayerEnteredDoor(Direction direction)
	{
		GD.Print("Current pos", currentRoomPosition);
		Vector2I newPosition = CalculateNewPosition(direction);
		GD.Print("New pos", newPosition);

		if (dungeonLayout.ContainsKey(newPosition))
		{
			LoadRoom(newPosition, DirectionHelper.GetOppositeDirection(direction));
		}
		else
		{
			GD.PrintErr($"No room exists in direction {direction} from current room");
		}
	}

	private Vector2I CalculateNewPosition(Direction direction)
	{
		Vector2I newPosition = currentRoomPosition;
		switch (direction)
		{
			case Direction.UP: newPosition.Y--; break;
			case Direction.DOWN: newPosition.Y++; break;
			case Direction.LEFT: newPosition.X--; break;
			case Direction.RIGHT: newPosition.X++; break;
		}
		return newPosition;
	}
	
	public Vector2I GetCurrentRoomPosition()
	{
		return currentRoomPosition;
	}
	
	public Dictionary<Vector2I, Room> GetDungeonLayout()
	{
		return dungeonLayout;
	}
}
