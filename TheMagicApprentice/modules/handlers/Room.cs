
public enum RoomType
{
    Entry,
    Normal,
    Boss,
    Treasure
}

public class Room
{
    public RoomType Type { get; set; }
    public string ScenePath { get; set; }
    public bool IsVisited { get; set; }

    public Room(RoomType type, string scenePath)
    {
        Type = type;
        ScenePath = scenePath;
        IsVisited = false;
    }
}