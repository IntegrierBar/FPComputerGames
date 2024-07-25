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
        Dungeon generatedDungeon = DungeonGenerator.GenerateDungeon(minRooms, maxRooms);
        CopyFrom(generatedDungeon);
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
        CurrentRoomPosition = entrancePosition;
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
        CopyFrom(dungeon);
    }

    /**
    * Copy the properties of another dungeon.
    * 
    * @param dungeon The dungeon to copy.
    */
    private void CopyFrom(Dungeon dungeon)
    {
        Layout = new Dictionary<Vector2I, Room>(dungeon.Layout);
        EntrancePosition = dungeon.EntrancePosition;
        CurrentRoomPosition = dungeon.EntrancePosition;
        BossPosition = dungeon.BossPosition;
        GridSize = dungeon.GridSize;
        MagicType = dungeon.MagicType;
    }
}
