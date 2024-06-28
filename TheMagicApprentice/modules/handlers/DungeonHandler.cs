using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonHandler : Node
{
	[Export]
	public int MinRooms = 5;
	[Export]
	public int MaxRooms = 10;
	
	private Dungeon dungeon;
	private RoomHandler roomHandler;
	private Node2D player;

	public override void _Ready()
	{
		player = GetTree().GetFirstNodeInGroup("player") as CharacterBody2D;
		roomHandler = GetTree().GetFirstNodeInGroup("room_handler") as RoomHandler;
		//dungeon = new Dungeon(MinRooms, MaxRooms);
		dungeon = new Dungeon(Dungeons.IntroDungeon);
		LoadRoom(dungeon.CurrentRoomPosition, Direction.DOWN);
	}

	private void LoadRoom(Vector2I position, Direction enterDirection)
	{
		if (!dungeon.Layout.ContainsKey(position))
		{
			GD.PrintErr($"Attempted to load non-existent room at position {position}");
			return;
		}

		Room room = dungeon.Layout[position];

		roomHandler.LoadRoom(room, enterDirection);
		room.IsVisited = true;
		dungeon.Layout[position] = room;  // Update the room in the dictionary
		dungeon.CurrentRoomPosition = position;

		ConnectRoomExitSignals();
	}

	private void ConnectRoomExitSignals()
	{
		foreach (Node child in roomHandler.CurrentRoomNode.GetChildren())
		{
			if (child is RoomExit exit)
			{
				exit.PlayerEnteredDoor += OnPlayerEnteredDoor;
			}
		}
	}

	private Direction CalculateEnterDirection(Vector2I newPosition)
	{
		Vector2I delta = newPosition - dungeon.CurrentRoomPosition;
		if (delta.X > 0) return Direction.LEFT;
		if (delta.X < 0) return Direction.RIGHT;
		if (delta.Y > 0) return Direction.UP;
		if (delta.Y < 0) return Direction.DOWN;
		return Direction.RIGHT;  // Default direction if same position
	}

	private void OnPlayerEnteredDoor(Direction direction)
	{
		Vector2I newPosition = CalculateNewPosition(direction);
		
		if (dungeon.Layout.ContainsKey(newPosition))
		{
			LoadRoom(newPosition, DirectionHelper.GetOppositeDirection(direction));
		}
		else
		{
			GD.PrintErr($"No room exists in direction {direction} from current room");
		}
	}

	private Vector2I CalculateNewPosition(Direction direction)
	{
		Vector2I newPosition = dungeon.CurrentRoomPosition;
		switch (direction)
		{
			case Direction.UP: newPosition.Y--; break;
			case Direction.DOWN: newPosition.Y++; break;
			case Direction.LEFT: newPosition.X--; break;
			case Direction.RIGHT: newPosition.X++; break;
		}
		return newPosition;
	}
	
	public Vector2I GetCurrentRoomPosition()
	{
		return dungeon.CurrentRoomPosition;
	}

	public Vector2 GetGridSize()
	{
		return dungeon.GridSize;
	}
	
	public Dictionary<Vector2I, Room> GetDungeonLayout()
	{
		return dungeon.Layout;
	}
}
