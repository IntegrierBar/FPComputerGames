namespace Tests;

using GdUnit4;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static GdUnit4.Assertions;

[TestSuite]
public partial class TestDungeonGenerator
{
	[TestCase]
	public void TestDungeonGeneration()
	{
		Dungeon dungeon;
		HashSet<string> uniqueLayouts = new HashSet<string>();

		for (int i = 0; i < 1000; i++)
		{
			dungeon = DungeonGenerator.GenerateDungeon(5, 10);

			AssertThat(dungeon.Layout).IsNotNull();
			AssertThat(dungeon.Layout.Count).IsBetween(5, 10);
			AssertThat(dungeon.EntrancePosition).IsNotEqual(dungeon.BossPosition);
			AssertThat(dungeon.CurrentRoomPosition).IsEqual(dungeon.EntrancePosition);

			// Check if there are at least 2 rooms between entrance and boss
			int manhattanDistance = Math.Abs(dungeon.EntrancePosition.X - dungeon.BossPosition.X) +
									Math.Abs(dungeon.EntrancePosition.Y - dungeon.BossPosition.Y);
			AssertThat(manhattanDistance).IsGreaterEqual(3);

			// Check if all rooms are within the grid
			foreach (var position in dungeon.Layout.Keys)
			{
				AssertThat(position.X).IsBetween(0, dungeon.GridSize.X - 1);
				AssertThat(position.Y).IsBetween(0, dungeon.GridSize.Y - 1);
			}

			// Check if entrance and boss rooms are correctly set
			AssertThat(dungeon.Layout[dungeon.EntrancePosition].Type).IsEqual(RoomType.Normal);
			AssertThat(dungeon.Layout[dungeon.BossPosition].Type).IsEqual(RoomType.Boss);

				string layoutHash = GetLayoutHash(dungeon.Layout);
				uniqueLayouts.Add(layoutHash);
			}

			// Check if we have generated multiple unique layouts
			AssertThat(uniqueLayouts.Count).IsGreater(1);
	}

	private string GetLayoutHash(Dictionary<Vector2I, Room> layout)
	{
		var sortedPositions = layout.Keys.OrderBy(pos => pos.X).ThenBy(pos => pos.Y);
		return string.Join(",", sortedPositions.Select(pos => $"{pos.X},{pos.Y},{layout[pos].Type},{layout[pos].ScenePath}"));
	}

	[TestCase]
	public void TestIsValidDungeonLayout()
	{
		// Use reflection to access private method
		var method = typeof(DungeonGenerator).GetMethod("IsValidDungeonLayout", 
			System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

		AssertThat(method).IsNotNull();

		AssertThat((bool)method.Invoke(null, new object[] { new Vector2I(0, 0), new Vector2I(3, 0) })).IsTrue();
		AssertThat((bool)method.Invoke(null, new object[] { new Vector2I(0, 0), new Vector2I(2, 0) })).IsFalse();
		AssertThat((bool)method.Invoke(null, new object[] { new Vector2I(0, 0), new Vector2I(1, 1) })).IsFalse();
		AssertThat((bool)method.Invoke(null, new object[] { new Vector2I(0, 0), new Vector2I(2, 1) })).IsTrue();
	}

	[TestCase]
	public void TestAddRoom()
	{
		var method = typeof(DungeonGenerator).GetMethod("AddRoom", 
			System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

		AssertThat(method).IsNotNull();

		Dictionary<Vector2I, Room> layout = new Dictionary<Vector2I, Room>();
		List<Vector2I> visitedTiles = new List<Vector2I> { new Vector2I(1, 1) };

		Vector2I newPos = (Vector2I)method.Invoke(null, new object[] { layout, new Vector2I(1, 1), visitedTiles });

		AssertThat(layout.Count).IsEqual(1);
		AssertThat(visitedTiles.Count).IsEqual(2);
		// Check that the new position is not the same as the original position
		AssertThat(newPos).IsNotEqual(new Vector2I(1, 1));
		// Check that the new position is adjacent to the original position
		AssertThat(Math.Abs(newPos.X - 1) + Math.Abs(newPos.Y - 1)).IsEqual(1);
		// Check that the new room is in the layout
		AssertThat(layout.ContainsKey(newPos)).IsTrue();
		// Check that the new room is of type Normal
		AssertThat(layout[newPos].Type).IsEqual(RoomType.Normal);
		// Check that the new position is in the visited tiles
		AssertThat(visitedTiles.Contains(newPos)).IsTrue();
	}

	[TestCase]
	public void TestGetShuffledDirections()
	{
		var method = typeof(DungeonGenerator).GetMethod("GetShuffledDirections", 
			System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

		AssertThat(method).IsNotNull();

		Godot.Collections.Array directions = (Godot.Collections.Array)method.Invoke(null, null);

		AssertThat(directions.Count).IsEqual(4);
		AssertThat(directions).Contains(new Vector2I(1, 0));
		AssertThat(directions).Contains(new Vector2I(-1, 0));
		AssertThat(directions).Contains(new Vector2I(0, 1));
		AssertThat(directions).Contains(new Vector2I(0, -1));
	}

	[TestCase]
	public void TestGetNextPosition()
	{
		// First, call GenerateDungeon to ensure GridSize is set
		DungeonGenerator.GenerateDungeon(5, 10);

		var method = typeof(DungeonGenerator).GetMethod("GetNextPosition", 
			System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

		AssertThat(method).IsNotNull();

		Vector2I gridSize = new Vector2I(2 * 10 + 1, 2 * 10 + 1);

		// Test with the actual GridSize
		Vector2I nextPos = (Vector2I)method.Invoke(null, new object[] { new Vector2I(gridSize.X / 2, gridSize.Y / 2), new Vector2I(1, 0) });
		AssertThat(nextPos.X).IsEqual(gridSize.X / 2 + 1);
		AssertThat(nextPos.Y).IsEqual(gridSize.Y / 2);

		// Test edge cases
		nextPos = (Vector2I)method.Invoke(null, new object[] { new Vector2I(0, 0), new Vector2I(-1, 0) });
		AssertThat(nextPos).IsEqual(new Vector2I(0, 0));

		nextPos = (Vector2I)method.Invoke(null, new object[] { new Vector2I(gridSize.X - 1, gridSize.Y - 1), new Vector2I(1, 1) });
		AssertThat(nextPos).IsEqual(new Vector2I(gridSize.X - 1, gridSize.Y - 1));
	}

	[TestCase]
	public void TestGetRandomRoomScene()
	{
		var method = typeof(DungeonGenerator).GetMethod("GetRandomRoomScene", 
			System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

		AssertThat(method).IsNotNull();

		string roomScene = (string)method.Invoke(null, null);
		AssertThat(roomScene == "res://modules/rooms/Room3.tscn" || roomScene == "res://modules/rooms/Room4.tscn").IsTrue();
	}
}
