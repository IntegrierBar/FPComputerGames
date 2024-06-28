using System.Collections.Generic;
using Godot;

/**
 * \class Dungeons
 * \brief Static class containing predefined dungeons.
 */
public static class Dungeons
{
    public static Dungeon IntroDungeon => new Dungeon(
        layout: new Dictionary<Vector2I, Room>
        {
            { new Vector2I(0, 0), new Room(RoomType.Normal, "res://modules/rooms/Room3.tscn") },
            { new Vector2I(0, 1), new Room(RoomType.Normal, "res://modules/rooms/Room4.tscn") },
            { new Vector2I(0, 2), new Room(RoomType.Normal, "res://modules/rooms/Room3.tscn") },
            { new Vector2I(0, 3), new Room(RoomType.Boss, "res://modules/rooms/Room4.tscn") }
        },
        entrancePosition: new Vector2I(0, 0),
        bossPosition: new Vector2I(0, 3),
        gridSize: new Vector2I(1, 4),
        magicType: MagicType.SUN
    );

    public static Dictionary<int, Dungeon> StoryDungeons => new Dictionary<int, Dungeon>
    {
        { 1, StoryDungeon1 }
    };

    private static Dungeon StoryDungeon1 => new Dungeon(
        layout: new Dictionary<Vector2I, Room>
        {
            { new Vector2I(1, 1), new Room(RoomType.Normal, "res://modules/rooms/Room3.tscn") },
            { new Vector2I(1, 2), new Room(RoomType.Normal, "res://modules/rooms/Room4.tscn") },
            { new Vector2I(2, 2), new Room(RoomType.Normal, "res://modules/rooms/Room3.tscn") },
            { new Vector2I(3, 2), new Room(RoomType.Normal, "res://modules/rooms/Room4.tscn") },
            { new Vector2I(3, 1), new Room(RoomType.Normal, "res://modules/rooms/Room3.tscn") },
            { new Vector2I(4, 1), new Room(RoomType.Normal, "res://modules/rooms/Room4.tscn") },
            { new Vector2I(5, 1), new Room(RoomType.Normal, "res://modules/rooms/Room3.tscn") },
            { new Vector2I(5, 2), new Room(RoomType.Boss, "res://modules/rooms/Room4.tscn") }
        },
        entrancePosition: new Vector2I(1, 1),
        bossPosition: new Vector2I(5, 2),
        gridSize: new Vector2I(7, 4),
        magicType: MagicType.COSMIC
    );
}