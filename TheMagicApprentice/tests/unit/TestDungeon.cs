namespace Tests;

using GdUnit4;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using static GdUnit4.Assertions;

[TestSuite]
public partial class TestDungeon
{
	[TestCase]
	public void TestDungeonConstructor()
	{
		Dungeon dungeon = new Dungeon(5, 10);
		AssertThat(dungeon.MinRooms).IsEqual(5);
		AssertThat(dungeon.MaxRooms).IsEqual(10);
		AssertThat(dungeon.Layout).IsNotNull();
		AssertThat(dungeon.Layout.Count).IsGreaterEqual(5);
	}

	[TestCase]
	public void TestDungeonGeneration()
	{
		Dungeon dungeon = new Dungeon(5, 10);

		AssertThat(dungeon.Layout).IsNotNull();
		AssertThat(dungeon.Layout.Count).IsBetween(5, 10);
		AssertThat(dungeon.EntrancePosition).IsNotEqual(dungeon.BossPosition);
		AssertThat(dungeon.CurrentRoomPosition).IsEqual(dungeon.EntrancePosition);

		// Check if there are at least 2 rooms between entrance and boss
		int manhattanDistance = Math.Abs(dungeon.EntrancePosition.X - dungeon.BossPosition.X) +
								Math.Abs(dungeon.EntrancePosition.Y - dungeon.BossPosition.Y);
		AssertThat(manhattanDistance).IsGreaterEqual(4);

		// Check if all rooms are within the grid
		foreach (var position in dungeon.Layout.Keys)
		{
			AssertThat(position.X).IsBetween(0, dungeon.GridSize.X - 1);
			AssertThat(position.Y).IsBetween(0, dungeon.GridSize.Y - 1);
		}

		// Check if entrance and boss rooms are correctly set
		AssertThat(dungeon.Layout[dungeon.EntrancePosition].Type).IsEqual(RoomType.Normal);
		AssertThat(dungeon.Layout[dungeon.BossPosition].Type).IsEqual(RoomType.Boss);
	}

	[TestCase]
	public void TestDungeonInitializationFromLayout()
	{
		// Create a sample layout
		Dictionary<Vector2I, Room> layout = new Dictionary<Vector2I, Room>
		{
			{ new Vector2I(0, 0), new Room(RoomType.Normal, "res://modules/rooms/Room3.tscn") },
			{ new Vector2I(0, 1), new Room(RoomType.Normal, "res://modules/rooms/Room4.tscn") },
			{ new Vector2I(1, 1), new Room(RoomType.Boss, "res://modules/rooms/Room3.tscn") }
		};
		Vector2I entrancePosition = new Vector2I(0, 0);
		Vector2I bossPosition = new Vector2I(1, 1);
		Vector2I gridSize = new Vector2I(2, 2);
		MagicType magicType = MagicType.COSMIC;

		// Create a dungeon using the layout constructor
		Dungeon dungeon = new Dungeon(layout, entrancePosition, bossPosition, gridSize, magicType);

		// Verify that the dungeon properties match the input
		AssertThat(dungeon.Layout).IsEqual(layout);
		AssertThat(dungeon.EntrancePosition).IsEqual(entrancePosition);
		AssertThat(dungeon.BossPosition).IsEqual(bossPosition);
		AssertThat(dungeon.GridSize).IsEqual(gridSize);
		AssertThat(dungeon.MagicType).IsEqual(magicType);

		// Verify that the current room position is set to the entrance
		AssertThat(dungeon.CurrentRoomPosition).Equals(entrancePosition);

		// Check if all rooms in the layout are present in the dungeon
		foreach (var kvp in layout)
		{
			AssertThat(dungeon.Layout.ContainsKey(kvp.Key)).IsTrue();
			AssertThat(dungeon.Layout[kvp.Key].Type).IsEqual(kvp.Value.Type);
			AssertThat(dungeon.Layout[kvp.Key].ScenePath).IsEqual(kvp.Value.ScenePath);
		}
	}

	[TestCase]
	public void TestMultipleDungeonGenerations()
	{
		Dungeon dungeon = new Dungeon(5, 10);
		HashSet<string> uniqueLayouts = new HashSet<string>();

		for (int i = 0; i < 100; i++)
		{
			dungeon.Generate();
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
}