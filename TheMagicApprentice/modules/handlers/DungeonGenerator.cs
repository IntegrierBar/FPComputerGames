using System;
using System.Collections.Generic;
using Godot;

public static class DungeonGenerator
{
	private static int MinRooms { get; set; } ///< Minimum number of rooms in the dungeon
	private static int MaxRooms { get; set; } ///< Maximum number of rooms in the dungeon
	private static Vector2I GridSize { get; set; } ///< Size of the grid used for dungeon generation

	/**
	* Generates the dungeon layout.
	* Clears the current layout and generates a new one with random rooms.
	* The number of rooms is set by the variables minRoom and maxRooms. These rooms are generated
	* in a grid like pattern, where each room is one cell. There are be at least 2 rooms
	* between the entrance of the dungeon and the boss room. The rooms themselves are not
	* randomly generated but randomly selected from a list of designed rooms.
	*/
	public static Dungeon GenerateDungeon(int minRooms, int maxRooms)
	{
		MinRooms = minRooms;
		MaxRooms = maxRooms;
		GridSize = new Vector2I(2 * MaxRooms + 1, 2 * MaxRooms + 1);

		Dictionary<Vector2I, Room> layout = new Dictionary<Vector2I, Room>();
		Vector2I entrancePosition = new Vector2I(GridSize.X / 2, GridSize.Y / 2);
		Vector2I bossPosition = entrancePosition;
		MagicType magicType = EntityTypeHelper.GetRandomMagicType();

		GenerateLayout(ref layout, ref entrancePosition, ref bossPosition);

		return new Dungeon(layout, entrancePosition, bossPosition, GridSize, magicType);
	}

    /**
	* Generates the layout of the dungeon.
	* Uses a random walk algorithm with a fail-safe mechanism to ensure a valid layout.
	* Continues generating layouts until a valid one is found (entrance and boss room are far enough apart).
	*/
	private static void GenerateLayout(ref Dictionary<Vector2I, Room> layout, ref Vector2I entrancePosition, ref Vector2I bossPosition)
	{
		int roomCount = GD.RandRange(MinRooms, MaxRooms);

		List<Vector2I> visitedTiles = new List<Vector2I>();

		while (!IsValidDungeonLayout(entrancePosition, bossPosition))
		{
			visitedTiles.Clear();
			visitedTiles.Add(entrancePosition);
			layout.Clear();
			layout[entrancePosition] = new Room(RoomType.Normal, GetRandomRoomScene());
			
			Vector2I currentPos = entrancePosition;

			for (int i = 0; i < roomCount - 1; i++)
			{
				currentPos = AddRoom(layout, currentPos, visitedTiles);
			}

			bossPosition = currentPos;
		}

		layout[bossPosition] = new Room(RoomType.Boss, "res://modules/rooms/BossRoom.tscn"); // set the boss room
	}

    /**
	* Checks if the current dungeon layout is valid.
	* A layout is considered valid if there are at least 2 rooms between the entrance and the boss room.
	*/
	private static bool IsValidDungeonLayout(Vector2I entrancePosition, Vector2I bossPosition)
	{
		return Math.Abs(entrancePosition.X - bossPosition.X) + Math.Abs(entrancePosition.Y - bossPosition.Y) > 2;
	}

    /**
	* Adds a new room to the dungeon layout.
	* Tries to add a room in a random direction from the current position.
	* If no valid direction is found, it returns to a previously visited tile.
	*/
	private static Vector2I AddRoom(Dictionary<Vector2I, Room> layout, Vector2I currentPos, List<Vector2I> visitedTiles)
	{
		Godot.Collections.Array directions = GetShuffledDirections();

		foreach (Vector2I direction in directions)
		{
			Vector2I nextPos = GetNextPosition(currentPos, direction);
			if (!layout.ContainsKey(nextPos))
			{
				layout[nextPos] = new Room(RoomType.Normal, GetRandomRoomScene());
				visitedTiles.Add(nextPos);
				return nextPos;
			}
		}

		return visitedTiles[GD.RandRange(0, visitedTiles.Count - 1)];
	}

    /**
	* Returns an array of shuffled directions (right, left, down, up).
	* Used to randomize the order of direction checking when adding new rooms.
	*/
	private static Godot.Collections.Array GetShuffledDirections()
	{
		Godot.Collections.Array directions = new Godot.Collections.Array{
			new Vector2I(1, 0),  // Move right
			new Vector2I(-1, 0), // Move left
			new Vector2I(0, 1),  // Move down
			new Vector2I(0, -1)  // Move up
		};
		directions.Shuffle();
		return directions;
	}

    /**
	* Calculates the next position based on the current position and a direction.
	* Ensures that the next position stays within the grid boundaries.
	*/
	private static Vector2I GetNextPosition(Vector2I currentPos, Vector2I direction)
	{
		Vector2I nextPos = currentPos + direction;
		nextPos.X = Math.Clamp(nextPos.X, 0, GridSize.X - 1);
		nextPos.Y = Math.Clamp(nextPos.Y, 0, GridSize.Y - 1);
		return nextPos;
	}

    /**
	* Returns a random room scene path.
	* Currently alternates between two predefined room scenes.
	*/
	private static string GetRandomRoomScene()
	{
		return GD.Randi() % 2 == 0 ? "res://modules/rooms/Room3.tscn" : "res://modules/rooms/Room4.tscn";
	}
}
