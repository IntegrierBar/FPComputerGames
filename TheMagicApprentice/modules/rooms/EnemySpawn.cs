using Godot;

public partial class EnemySpawn : Node2D
{
    public Node2D Spawn()
    {
        Node2D Slime = ResourceLoader.Load<PackedScene>("res://modules/entities/slimes/slime.tscn").Instantiate() as Node2D;
        Slime.GlobalPosition = GlobalPosition;
        RoomHandler RoomHandler = GetTree().GetFirstNodeInGroup("room_handler") as RoomHandler;
        RoomHandler.CurrentRoomNode.AddChild(Slime);
        return Slime;
    }
}
