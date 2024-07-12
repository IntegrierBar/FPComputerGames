using System.Collections.Generic;
using System;
using Godot;
using System.Net.Http.Headers;

public class Dungeon
{
    public Dictionary<Vector2I, Room> Layout { get; private set; } ///< Layout of the dungeon, where each room is mapped to a position.
    public Vector2I CurrentRoomPosition { get; set; } ///< Current room position in the dungeon.
    public Vector2I EntrancePosition { get; private set; } ///< Entrance position of the dungeon.
    public Vector2I BossPosition { get; private set; } ///< Boss room position in the dungeon.
    public int MinRooms { get; private set; } ///< Minimum number of rooms in the dungeon.
    public int MaxRooms { get; private set; } ///< Maximum number of rooms in the dungeon.
    public Vector2I GridSize { get; private set; } ///< Size of the dungeon grid.
    public MagicType MagicType { get; private set; } ///< Magic type of the dungeon.

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
        GridSize = new Vector2I(2 * MaxRooms + 1, 2 * MaxRooms + 1);
        Layout = new Dictionary<Vector2I, Room>();
        Generate();
    }

    /**
    * Constructor for the Dungeon class.
    * Initializes the dungeon with a given layout, entrance position, boss position, grid size, and magic type.
    * 
    * @param layout Dictionary mapping positions to rooms in the dungeon.
    * @param entrancePosition Position of the entrance in the dungeon.
    * @param bossPosition Position of the boss room in the dungeon.
    * @param gridSize Size of the dungeon grid.
    * @param magicType Magic type of the dungeon.
    */
    public Dungeon(Dictionary<Vector2I, Room> layout, Vector2I entrancePosition, Vector2I bossPosition, Vector2I gridSize, MagicType magicType)
    {
        Layout = layout;
        EntrancePosition = entrancePosition;
        BossPosition = bossPosition;
        GridSize = gridSize;
        MagicType = magicType;
    }

    /**
    * Copy constructor for the Dungeon class.
    * Initializes the dungeon by copying the properties of another dungeon.
    * 
    * @param dungeon The dungeon to copy.
    */
    public Dungeon(Dungeon dungeon)
    {
        Layout = dungeon.Layout;
        EntrancePosition = dungeon.EntrancePosition;
        CurrentRoomPosition = EntrancePosition;
        BossPosition = dungeon.BossPosition;
        GridSize = dungeon.GridSize;
        MagicType = dungeon.MagicType;
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
        MagicType = EntityTypeHelper.GetRandomMagicType();

        Layout.Clear();
        int roomCount = GD.RandRange(MinRooms, MaxRooms);
        EntrancePosition = new Vector2I(GridSize.X / 2, GridSize.Y / 2);
        BossPosition = EntrancePosition;
        CurrentRoomPosition = EntrancePosition;

        // Repeat until you have a dungeon with a boss room that is at least 2 rooms away from the entrance.
        while (Math.Abs(EntrancePosition.X - BossPosition.X) + Math.Abs(EntrancePosition.Y - BossPosition.Y) <= 2)
        {
            Layout = new Dictionary<Vector2I, Room>();
            Layout[EntrancePosition] = new Room(RoomType.Normal, GetRandomRoomScene());

            List<Vector2I> visitedTiles = new List<Vector2I> { EntrancePosition };
            Vector2I currentPos = EntrancePosition;
            // From the current position, walk randomly in one direction and add the room to the layout.
            // If the next room is already in the layout, walk in a different direction.
            // Repeat this process until the boss room is added to the layout.
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
