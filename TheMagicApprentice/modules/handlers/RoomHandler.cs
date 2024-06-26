using Godot;
using System.Collections.Generic;



public partial class RoomHandler : Node
{
	[Signal]
	public delegate void RoomInitializedEventHandler(); ///< Signal emitted after a new room is loaded.
	
	public Node currentRoom { private set; get; } ///< Reference to the current room node. Has to contain a "TileMap" node.
	private TileMap currentTileMap; ///< Reference to the current tile map node
	private CharacterBody2D player; ///< Reference to the player node

	/**
	 * Called when the node is added to the scene.
	 * Initializes the player and room handler, and loads the initial room.
	 */
	public override void _Ready()
	{
		player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		System.Diagnostics.Debug.Assert(player != null);
	}

	/**
	 * Loads a room by name and sets the player's position based on the entrance direction.
	 * 
	 * @param roomName The name of the room to load.
	 * @param enterDirection The direction from which the player enters the room.
	 */
	public void LoadRoom(string scenePath, Direction enterDirection)
	{
		if(player == null)
		{
			player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;	
		}
		if (currentRoom != null)
		{
			currentRoom.QueueFree();  // Remove the current room
		}

		// Load the new room
		currentRoom = ResourceLoader.Load<PackedScene>(scenePath).Instantiate();
		currentTileMap = currentRoom.GetNode<TileMap>("TileMap");
		System.Diagnostics.Debug.Assert(currentTileMap is not null, "TileMap node not found in the room scene.");
		AddChild(currentRoom);
		
		// Initialize the new room (e.g., spawn enemies)
		InitializeRoom(enterDirection);
	}
	
	/**
	 * Initializes the current room and emits a signal when the room is initialized.
	 */
	private void InitializeRoom(Direction enterDirection)
	{
		// Set player position based on entrance
		foreach (Node child in currentRoom.GetChildren())
		{
			if (child is RoomEntrance entrance && entrance.Direction == enterDirection)
			{
				player.Position = entrance.Position;
				break;
			}
		}

		GetTree().CreateTimer(0.1f).Timeout += () =>
		{
			foreach (Node child in currentRoom.GetChildren())
			{
				if (child is RoomExit exit)
				{
					exit.RegisterExit();
				}
			}
		};

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
