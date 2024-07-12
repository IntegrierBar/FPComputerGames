using Godot;
using System;
using System.Collections.Generic;



public partial class RoomHandler : Node
{
	[Signal]
	public delegate void RoomInitializedEventHandler(); ///< Signal emitted after a new room is loaded.
	[Export]
	public int EnemyCount { get; set; } = 3; ///< Number of enemies to spawn in each room.
	
	public Room CurrentRoom { private set; get; } ///< Reference to the current room object.
	public Node CurrentRoomNode { private set; get; } ///< Reference to the current room node. Has to contain a "TileMap" node.
	private TileMap currentTileMap; ///< Reference to the current tile map node
	private CharacterBody2D player; ///< Reference to the player node
	private int enemyCount = 0; ///< Number of enemies in the current room

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
	public void LoadRoom(Room room, Direction enterDirection)
	{
		if(player == null)
		{
			player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;	
		}
		if (CurrentRoom != null)
		{
			CurrentRoomNode.QueueFree();  // Remove the current room
		}

		// Load the new room
		CurrentRoom = room;
		CurrentRoomNode = ResourceLoader.Load<PackedScene>(room.ScenePath).Instantiate();
		currentTileMap = CurrentRoomNode.GetNode<TileMap>("TileMap");
		System.Diagnostics.Debug.Assert(currentTileMap is not null, "TileMap node not found in the room scene.");
		AddChild(CurrentRoomNode);
		
		// Initialize the new room (e.g., spawn enemies)
		InitializeRoom(enterDirection);
	}
	
	/**
	 * Initializes the current room and emits a signal when the room is initialized.
	 */
	private void InitializeRoom(Direction enterDirection)
	{
		// Set player position based on entrance
		foreach (Node child in CurrentRoomNode.GetChildren())
		{
			if (child is RoomEntrance entrance && entrance.Direction == enterDirection)
			{
				player.Position = entrance.Position;
				break;
			}
		}

		if(CurrentRoom.IsCleared)
			EnableRoomExits();

		if(!CurrentRoom.IsCleared)
			SpawnEnemies();

		EmitSignal(SignalName.RoomInitialized);
	}

	/**
	 * Spawns enemies based on the nodes that inherit from the EnemySpawn class.
	 */
	private void SpawnEnemies()
	{
		var enemySpawns = new List<EnemySpawn>();
		foreach (Node child in CurrentRoomNode.GetChildren())
		{
			if (child is EnemySpawn spawn)
			{
				enemySpawns.Add(spawn);
			}
		}

		var random = new Random();
		int enemiesToSpawn = EnemyCount;
		if (CurseHandler.IsActive(Curse.MORE_MONSTERS))
		{
			enemiesToSpawn = (int)Math.Round(EnemyCount * 1.333);
		}

		for (int i = 0; i < enemiesToSpawn && enemySpawns.Count > 0; i++)
		{
			int index = random.Next(enemySpawns.Count);
			Node2D enemy = enemySpawns[index].Spawn();
			if (enemy.GetNode("HealthComponent") is HealthComponent healthComponent)
			{
				healthComponent.Death += EnemyDied;
			}
			enemySpawns.RemoveAt(index);
		}
		enemyCount = enemiesToSpawn;
	}

	/**
	 * Called when an enemy dies. Once all enemies are dead, the room is cleared and the room exits are enabled.
	 */
	private void EnemyDied()
	{
		enemyCount--;
		if(enemyCount == 0) {
			CurrentRoom.IsCleared = true;
			EnableRoomExits();
		}
	}
	/**
	* Enables the Rooms Exits.
	*/
	private void EnableRoomExits()
	{
		GetTree().CreateTimer(0.1f).Timeout += () =>
		{
			foreach (Node child in CurrentRoomNode.GetChildren())
			{
				if (child is RoomExit exit)
				{
					exit.RegisterExit();
				}
			}
		};
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
