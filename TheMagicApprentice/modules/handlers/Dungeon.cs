using System.Collections.Generic;
using System;
using Godot;

public class Dungeon
{
	public Dictionary<Vector2I, Room> Layout { get; private set; } ///< Layout of the dungeon, where each room is mapped to a position.
	public Vector2I CurrentRoomPosition { get; set; } ///< Current room position in the dungeon.
	public Vector2I EntrancePosition { get; private set; } ///< Entrance position of the dungeon.
	public Vector2I BossPosition { get; private set; } ///< Boss room position in the dungeon.
	public int MinRooms { get; private set; } ///< Minimum number of rooms in the dungeon.
	public int MaxRooms { get; private set; } ///< Maximum number of rooms in the dungeon.
	public Vector2I GridSize => new Vector2I(2 * MaxRooms + 1, 2 * MaxRooms + 1); ///< Size of the dungeon grid.

	/**
	* Constructor for the Dungeon class.
	* Initializes the minimum and maximum number of rooms and the layout.
	* 
	* @param minRooms Minimum number of rooms in the dungeon.
	* @param maxRooms Maximum number of rooms in the dungeon.
	*/
	public Dungeon(int minRooms, int maxRooms)
	{
		MinRooms = minRooms;
		MaxRooms = maxRooms;
		Layout = new Dictionary<Vector2I, Room>();
	}

	/**
	* Generates the dungeon layout.
	* Clears the current layout and generates a new one with random rooms.
	* Every generated dungeon has between 5 and 10 rooms. These rooms are generated
	* in a grid like pattern, where each room is one cell. There are be at least 2 rooms
	* between the entrance of the dungeon and the boss room. The rooms themselves are not
	* randomly generated but randomly selected from a list of designed rooms.
	*/
	public void Generate()
	{
		Layout.Clear();
		int roomCount = GD.RandRange(MinRooms, MaxRooms);
		EntrancePosition = new Vector2I(GridSize.X / 2, GridSize.Y / 2);
		BossPosition = EntrancePosition;
		CurrentRoomPosition = EntrancePosition;

		while (Math.Abs(EntrancePosition.X - BossPosition.X) + Math.Abs(EntrancePosition.Y - BossPosition.Y) < 4)
		{
			Layout = new Dictionary<Vector2I, Room>();
			Layout[EntrancePosition] = new Room(RoomType.Normal, GetRandomRoomScene());

			List<Vector2I> visitedTiles = new List<Vector2I> { EntrancePosition };
			Vector2I currentPos = EntrancePosition;
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
					if (!Layout.ContainsKey(nextPos))
					{
						Layout[nextPos] = new Room(RoomType.Normal, GetRandomRoomScene());
						visitedTiles.Add(nextPos);
						currentPos = nextPos;
						moved = true;
						break;
					}
				}

				if (!moved)
				{
					currentPos = visitedTiles[(int)GD.Randi() % visitedTiles.Count];
				}
			}

			BossPosition = currentPos;
		}

		Layout[BossPosition] = new Room(RoomType.Boss, GetRandomRoomScene());
	}

	/**
	* Gets a random room scene path.
	* 
	* @return A string representing the path to a random room scene.
	*/
	private string GetRandomRoomScene()
	{
		return GD.Randi() % 2 == 0 ? "res://modules/rooms/Room3.tscn" : "res://modules/rooms/Room4.tscn";
	}
}
