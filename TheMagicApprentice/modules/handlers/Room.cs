
using System.Text.Json.Serialization;

public enum RoomType
{
    Normal,
    Boss
}

public class Room
{
    public RoomType Type { get; set; }
    public string ScenePath { get; set; }
    public bool IsVisited { get; set; }
    public bool IsCleared { get; set; }

    /**
    * Parameterless constructor for JSON deserialization.
    */
    [JsonConstructor]
    public Room() { }

    public Room(RoomType type, string scenePath)
    {
        Type = type;
        ScenePath = scenePath;
        IsVisited = false;
        IsCleared = false;
    }
}