using Godot;
using System.Collections.Generic;

public partial class DungeonHandler : Node
{
	private Dictionary<string, PackedScene> rooms = new Dictionary<string, PackedScene>
	{
		{ "Room1", (PackedScene)ResourceLoader.Load("res://modules/rooms/Room1.tscn") },
		{ "Room2", (PackedScene)ResourceLoader.Load("res://modules/rooms/Room2.tscn") }
	};

	private Node currentRoom; ///< Reference to the current room node
	private Node2D player; ///< Reference to the player node
	private Node roomHandler; ///< Reference to the room handler node

	/**
	 * Called when the node is added to the scene.
	 * Initializes the player and room handler, and loads the initial room.
	 */
	public override void _Ready()
	{
		GD.Print("Tree", GetTree());
		player = GetTree().GetNodesInGroup("player")[0] as Node2D;
		roomHandler = GetTree().GetNodesInGroup("room_handler")[0];
		LoadRoom("Room1", Direction.RIGHT);
	}

	/**
	 * Loads a room by name and sets the player's position based on the entrance direction.
	 * 
	 * @param roomName The name of the room to load.
	 * @param enterDirection The direction from which the player enters the room.
	 */
	private void LoadRoom(string roomName, Direction enterDirection)
	{
		if (currentRoom != null)
		{
			currentRoom.Free();  // Remove the current room
		}

		// Load the new room
		currentRoom = rooms[roomName].Instantiate();
		AddChild(currentRoom);

		// Initialize the new room (e.g., spawn enemies)
		roomHandler.Call("InitializeRoom");

		// Connect door signals in the new room
		foreach (Node child in currentRoom.GetChildren())
		{
			if (child is RoomExit door)
			{
				door.PlayerEnteredDoor += OnPlayerEnteredDoor;
			}
			if (child is RoomEntrance entrance && entrance.Direction == enterDirection)
			{
				player.Position = entrance.Position;
			}
		}
	}

	/**
	 * Callback for when the player enters a door. The DungeonHandler then loads a new room depending on the direction the player entered the exit.
	 * 
	 * @param targetRoom The name of the target room to load.
	 * @param direction The direction from which the player entered the door.
	 */
	private void OnPlayerEnteredDoor(string targetRoom, Direction direction)
	{
		GD.Print("Player entered door ", targetRoom, direction);
		GD.Print("Player position: ", player.Position);
		LoadRoom(targetRoom, DirectionHelper.GetOppositeDirection(direction));
		GD.Print("Player position: ", player.Position);
	}
}
