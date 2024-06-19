using Godot;
using System.Collections.Generic;

public partial class RoomHandler : Node
{
	[Signal]
	public delegate void RoomInitializedEventHandler(); ///< Signal emitted after a new room is loaded.
	
	private Dictionary<string, PackedScene> rooms = new Dictionary<string, PackedScene>
	{
		{ "Room1", (PackedScene)ResourceLoader.Load("res://modules/rooms/Room1.tscn") },
		{ "Room2", (PackedScene)ResourceLoader.Load("res://modules/rooms/Room2.tscn") }
	};

	private Node currentRoom; ///< Reference to the current room node. Has to contain a "TileMap" node.
	private TileMap currentTileMap; ///< Reference to the current tile map node
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
			currentRoom.QueueFree();  // Remove the current room
		}

		// Load the new room
		currentRoom = rooms[roomName].Instantiate();
		currentTileMap = currentRoom.GetNode<TileMap>("TileMap");
		System.Diagnostics.Debug.Assert(currentTileMap is not null);
		AddChild(currentRoom);

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
		
		// Initialize the new room (e.g., spawn enemies)
		InitializeRoom();
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
	
	/**
	 * Initializes the current room and emits a signal when the room is initialized.
	 */
	private void InitializeRoom()
	{
		EmitSignal(SignalName.RoomInitialized);
	}

	/**
	 * Gets the current room bounds in real-world coordinates.
	 * 
	 * @return The current room bounds in real-world coordinates.
	 */
	public Rect2 GetCurrentRoomBounds()
	{
		Rect2 tileBounds = currentTileMap.GetUsedRect();
		Vector2 cellSize = currentTileMap.TileSet.TileSize;

		// Convert tile bounds to real-world coordinates
		Vector2 realPosition = tileBounds.Position * cellSize;
		Vector2 realSize = tileBounds.Size * cellSize;

		return new Rect2(realPosition, realSize);
	}
}
