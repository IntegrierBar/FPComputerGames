using System.Collections.Generic;
using System;
using Godot;
using System.Net.Http.Headers;
using System.Linq;
using System.Text.Json.Serialization;


public class Dungeon
{
    public string Name { get; set; } ///< Name of the dungeon.
    public bool IsStoryDungeon { get; set; } ///< Whether this is a story dungeon
    public int StoryIndex { get; set; } = -1; ///< Index of story dungeon (0-4), -1 if not a story dungeon

    [JsonIgnore] // Ignore the layout from serialization
    public Dictionary<Vector2I, Room> Layout { get; set; } ///< Layout of the dungeon, where each room is mapped to a position.
    
    public Vector2I CurrentRoomPosition { get; set; } ///< Current room position in the dungeon.
    public Vector2I EntrancePosition { get; set; } ///< Entrance position of the dungeon.
    public Vector2I BossPosition { get; set; } ///< Boss room position in the dungeon.
    public Vector2I GridSize { get; set; } ///< Size of the dungeon grid.
    public MagicType MagicType { get; set; } ///< Magic type of the dungeon.

    /**
    * Parameterless constructor for JSON deserialization.
    */
    [JsonConstructor]
    public Dungeon()
    {
        Layout = new Dictionary<Vector2I, Room>();
        IsStoryDungeon = false;
        StoryIndex = -1;
    }

    /**
    * Constructor for the Dungeon class.
    * Initializes the minimum and maximum number of rooms and the layout.
    * 
    * @param minRooms Minimum number of rooms in the dungeon.
    * @param maxRooms Maximum number of rooms in the dungeon.
    */
    public Dungeon(int minRooms, int maxRooms)
    {
        Name = "GeneratedDungeon";
        IsStoryDungeon = false;
        StoryIndex = -1;
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
        Name = "GeneratedDungeon";
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
        Name = dungeon.Name;
        IsStoryDungeon = dungeon.IsStoryDungeon;
        StoryIndex = dungeon.StoryIndex;
        Layout = new Dictionary<Vector2I, Room>(dungeon.Layout);
        EntrancePosition = dungeon.EntrancePosition;
        CurrentRoomPosition = dungeon.EntrancePosition;
        BossPosition = dungeon.BossPosition;
        GridSize = dungeon.GridSize;
        MagicType = dungeon.MagicType;
    }

    // Serializable version of the layout
    public SerializableRoom[] SerializableLayout
    {
        get => Layout?.Select(kvp => new SerializableRoom 
        { 
            X = kvp.Key.X, 
            Y = kvp.Key.Y, 
            Type = kvp.Value.Type,
            ScenePath = kvp.Value.ScenePath 
        }).ToArray();
        set
        {
            Layout = value?.ToDictionary(
                sr => new Vector2I(sr.X, sr.Y),
                sr => new Room(sr.Type, sr.ScenePath)
            );
        }
    }
}

public class SerializableRoom
{
    public int X { get; set; }
    public int Y { get; set; }
    public RoomType Type { get; set; }
    public string ScenePath { get; set; }
}