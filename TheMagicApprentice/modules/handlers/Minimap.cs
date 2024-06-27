using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// A UI element that displays a minimap of the dungeon.
/// </summary>
public partial class Minimap : Control
{
	[Export]
	public Vector2I CellSize = new Vector2I(10, 10); ///< Size of each cell in the minimap grid.

	private DungeonHandler dungeonHandler;
	private Dictionary<Vector2I, Room> dungeonLayout;
	private Vector2I currentRoomPosition;

	/// <summary>
	/// Called when the node enters the scene tree for the first time.
	/// Initializes references to the DungeonHandler and retrieves the dungeon layout.
	/// </summary>
	public override void _Ready()
	{
		dungeonHandler = GetTree().GetFirstNodeInGroup("dungeon_handler") as DungeonHandler;
		dungeonLayout = dungeonHandler.GetDungeonLayout();
		currentRoomPosition = dungeonHandler.GetCurrentRoomPosition();
	}

	/// <summary>
	/// Called every frame. Updates the minimap display.
	/// </summary>
	/// <param name="delta">The time elapsed since the last frame.</param>
	public override void _Process(double delta)
	{
		currentRoomPosition = dungeonHandler.GetCurrentRoomPosition();
		QueueRedraw();
	}

	/// <summary>
	/// Draws the minimap.
	/// </summary>
	public override void _Draw()
	{
		// Draw black background
		DrawRect(new Rect2(Vector2.Zero, new Vector2(CellSize.X * dungeonHandler.GetGridSize().X, CellSize.Y * dungeonHandler.GetGridSize().Y)), Colors.Black);

		foreach (var kvp in dungeonLayout)
		{
			Vector2I gridPosition = kvp.Key;
			Room room = kvp.Value;

			Color roomColor = room.IsVisited ? Colors.White : Colors.Gray;
			if (room.Type == RoomType.Boss)
			{
				roomColor = Colors.Red;
			}
			if (gridPosition == currentRoomPosition)
			{
				roomColor = Colors.Blue;
			}

			Vector2 drawPosition = new Vector2(gridPosition.X * CellSize.X, gridPosition.Y * CellSize.Y);
			DrawRect(new Rect2(drawPosition, CellSize), roomColor);
		}
	}
}
